using Microsoft.UI.Xaml.Data;
using System;
using System.IO;

namespace HashCode
{

    enum HashStatus
    {
        None,
        Ready,
        Running,
        Canceled,
        Finished
    }

    class HashStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var status = (HashStatus)value;
            var type = parameter as string;
            return type switch
            {
                "Stopped" => status != HashStatus.Running,
                "RunningOnly" => status == HashStatus.Running,
                "CanRun" => status == HashStatus.Ready || status == HashStatus.Canceled || status == HashStatus.Finished,
                "FinishedOnly" => status == HashStatus.Finished,
                _ => false
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class FileConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value as FileInfo)?.FullName ?? string.Empty;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string path)
            {
                return new FileInfo(path);
            }
            return null;
        }
    }

    class FileHashInfo : BaseElement
    {
        private FileInfo? _file;
        private string _message;
        private HashStatus _status;
        private double _progress;
        private long _processed;

        public FileInfo? File
        {
            get => this._file;
            set
            {

                if (SetProperty(ref this._file, value))
                {
                    this._processed = 0;
                    this.Progress = 0;
                    if (this._file != null && this._file.Exists)
                    {
                        this.Status = HashStatus.Ready;
                    }
                    else
                    {
                        this.Status = HashStatus.None;
                    }
                }
            }
        }

        public string Message
        {
            get => this._message;
            set => SetProperty(ref this._message, value);
        }

        public HashStatus Status
        {
            get => this._status;
            set => SetProperty(ref this._status, value);
        }

        public double Progress
        {
            get => this._progress;
            private set => SetProperty(ref this._progress, value);
        }

        public FileHashInfo()
        {
            this._message = string.Empty;
            this._status = HashStatus.None;
            this._progress = 0;
            this._progress = 0;
            this._file = null;
        }

        public void Start()
        {
            this._processed = 0;
            this.Progress = 0;
            this.Status = HashStatus.Running;
            this.Message = $"FileName: {this.File!.FullName}\nSize: {FormatSize(this.File!.Length)}\n";
        }

        public void Update(long chunkSize)
        {
            this._processed += chunkSize;
            this.Progress = (double)this._processed / this.File!.Length * 100.0;
        }

        public void Finish(ReadOnlySpan<byte> md5, ReadOnlySpan<byte> sha1, ReadOnlySpan<byte> crc32, TimeSpan elapsed)
        {
            this.Status = HashStatus.Finished;
            this.Message += $"MD5: {BytesToString(md5)}\nSHA1: {BytesToString(sha1)}\nCRC32: {BytesToString(crc32)}\nTime used: {elapsed.TotalMilliseconds}ms";
        }

        public void Cancel()
        {
            this.Status = HashStatus.Canceled;
        }

        private static char ToCharUpper(int b)
        {
            b &= 0xF;
            return b switch
            {
                <= 9 => (char)(48 + b),
                _ => (char)(65 + b - 10)
            };
        }

        private unsafe static string BytesToString(ReadOnlySpan<byte> bytes)
        {
            fixed (byte* ptr = bytes)
            {
                return string.Create(bytes.Length * 2, (ptr: (nint)ptr, length: bytes.Length), (dst, state) =>
                {
                    var src = new ReadOnlySpan<byte>((byte*)state.ptr, state.length);
                    int i = 0;
                    int j = 0;

                    byte b = src[i++];
                    dst[j++] = ToCharUpper(b >> 4);
                    dst[j++] = ToCharUpper(b);

                    while (i < src.Length)
                    {
                        b = src[i++];
                        dst[j++] = ToCharUpper(b >> 4);
                        dst[j++] = ToCharUpper(b);
                    }
                });
            }
        }

        private static string FormatSize(long size) => size switch
        {
            > 1024 and <= 1048576 => $"{size / 1024.0:f2} KB",
            > 1048576 and <= 1073741824 => $"{size / 1024.0 / 1024.0:f2} MB",
            > 1073741824 => $"{size / 1024.0 / 1024.0 / 1024:f2} GB",
            _ => $"{size} B"
        };
    }
}
