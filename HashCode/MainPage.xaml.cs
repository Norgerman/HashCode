using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Norgerman.Hash;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.UI.Shell;
using Windows.Win32.UI.Shell.Common;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HashCode
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int BUFFER_SIZE = 1024 * 1024 * 16;

        private readonly FileHashInfo _hashInfo;
        private readonly Ticker _ticker;
        private CancellationTokenSource? _cancellationTokenSource;
        private byte[] _resultBuffer;

        public MainPage()
        {
            this._hashInfo = new FileHashInfo();
            this.InitializeComponent();
            this._ticker = new Ticker(new TimeSpan(10000));
            this._resultBuffer = new byte[40];
        }


        private async void Page_Drop(object _, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Count > 0)
                {
                    var storageFile = items[0] as StorageFile;
                    if (storageFile != null)
                    {
                        this._hashInfo.File = new FileInfo(storageFile.Path);
                        e.Handled = true;
                    }
                }
            }
        }

        private void Page_DragOver(object _, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Link;
        }


        private unsafe static string? PickFile(nint hWnd)
        {
            var hr = PInvoke.CoCreateInstance<IFileOpenDialog>(typeof(FileOpenDialog).GUID, null, CLSCTX.CLSCTX_INPROC_SERVER, out var fod);
            if (!hr.Succeeded)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
            var types = new COMDLG_FILTERSPEC
            {
                pszSpec = (char*)Marshal.StringToHGlobalUni("*.*"),
                pszName = (char*)Marshal.StringToHGlobalUni("All Files")
            };
            fod->SetFileTypes(1, &types);

            try
            {
                IShellItem* ppsi;
                fod->Show((HWND)hWnd);
                fod->GetResult(&ppsi);
                PWSTR filename;
                ppsi->GetDisplayName(SIGDN.SIGDN_FILESYSPATH, &filename);
                ppsi->Release();
                return filename.ToString();
            }
            catch
            {
                return null;
            }
            finally
            {
                fod->Release();
                Marshal.FreeHGlobal((nint)types.pszSpec.Value);
                Marshal.FreeHGlobal((nint)types.pszName.Value);
            }
        }

        private void Open_Click(object _, RoutedEventArgs e)
        {
            var window = ((Application.Current as App)?.Window as MainWindow);
            if (window != null)
            {
                var hWnd = WindowNative.GetWindowHandle(window);
                var result = PickFile(hWnd);
                if (result != null)
                {
                    this._hashInfo.File = new FileInfo(result);
                }
            }
        }

        private async void Start_Click(object _, RoutedEventArgs e)
        {
            this._hashInfo.Start();
            this._ticker.Start();
            var cancleTokenSource = new CancellationTokenSource();
            this._cancellationTokenSource = cancleTokenSource;
            var token = this._cancellationTokenSource.Token;
            await Task.Run(async () =>
            {
                var file = this._hashInfo.File;
                if (file != null)
                {
                    using var md5 = MD5.Create();
                    using var sha1 = SHA1.Create();
                    using var crc32 = new CRC32();
                    using var stream = file.OpenRead();
                    var buffer = new byte[BUFFER_SIZE];
                    var readed = 0;
                    try
                    {
                        while ((readed = await stream.ReadAsync(buffer, cancleTokenSource.Token)) != 0 && !cancleTokenSource.IsCancellationRequested)
                        {
                            var chunk = readed;
                            if (stream.Length > stream.Position)
                            {
                                md5.TransformBlock(buffer, 0, readed, buffer, 0);
                                sha1.TransformBlock(buffer, 0, readed, buffer, 0);
                                crc32.TransformBlock(buffer, 0, readed, buffer, 0);
                            }
                            else
                            {
                                md5.TransformFinalBlock(buffer, 0, readed);
                                sha1.TransformFinalBlock(buffer, 0, readed);
                                crc32.TransformFinalBlock(buffer, 0, readed);
                                md5.Hash.CopyTo(new Span<byte>(this._resultBuffer, 0, 16));
                                sha1.Hash.CopyTo(new Span<byte>(this._resultBuffer, 16, 20));
                                crc32.Hash.CopyTo(new Span<byte>(this._resultBuffer, 36, 4));

                            }
                            this.DispatcherQueue.TryEnqueue(() =>
                            {
                                this._hashInfo.Update(chunk);
                            });
                        }
                        if (!cancleTokenSource.IsCancellationRequested)
                        {
                            this.DispatcherQueue.TryEnqueue(() =>
                            {
                                this.OnFinish();
                            });
                        }
                    }
                    catch
                    {
                        //
                    }
                }
                cancleTokenSource.Dispose();
                this._cancellationTokenSource = null;
            }, token);
        }

        private void Copy_Click(object _, RoutedEventArgs e)
        {
            var data = new DataPackage();
            data.SetText(this._hashInfo.Message);
            Clipboard.SetContent(data);
        }

        private void Cancel_Click(object _, RoutedEventArgs e)
        {
            this._hashInfo.Cancel();
            this._ticker.Stop();
            this._cancellationTokenSource?.Cancel();
            this._cancellationTokenSource = null;
        }

        private void OnFinish()
        {
            this._ticker.Stop();
            this._hashInfo.Finish(new Span<byte>(this._resultBuffer, 0, 16), new Span<byte>(this._resultBuffer, 16, 20), new Span<byte>(this._resultBuffer, 36, 4), this._ticker.Elapsed);
        }
    }
}
