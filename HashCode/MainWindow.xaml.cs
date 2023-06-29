using Microsoft.UI.Xaml;
using System;
using System.Drawing;
using Windows.Graphics;
using Windows.Win32.Foundation;
using WinRT.Interop;
using static Windows.Win32.PInvoke;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HashCode
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.SetIcon();
            this.AppWindow.Resize(new SizeInt32(640, 480));
        }

        private void SetIcon()
        {
            var hwnd = WindowNative.GetWindowHandle(this);
            if (Environment.ProcessPath != null)
            {
                var icon = Icon.ExtractAssociatedIcon(Environment.ProcessPath);
                if (icon != null)
                {
                    SendMessage((HWND)hwnd, WM_SETICON, ICON_BIG, icon.Handle);
                }
            }
        }
    }
}
;