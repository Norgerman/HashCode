using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace HashCode
{
    class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var timeSpan = (TimeSpan)value;
            return timeSpan.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class Ticker : BaseElement
    {
        private readonly DispatcherTimer _timer;
        private TimeSpan _elapsed;
        private DateTimeOffset _start;

        public TimeSpan Elapsed
        {
            get => this._elapsed;
            set => SetProperty(ref this._elapsed, value);
        }

        public Ticker(TimeSpan interval)
        {
            this._timer = new DispatcherTimer()
            {
                Interval = interval
            };
            this._timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, object e)
        {
            this.Elapsed = DateTimeOffset.Now - this._start;
        }

        public void Start()
        {
            this._timer.Start();
            this.Elapsed = new TimeSpan(0);
            this._start = DateTimeOffset.Now;
        }

        public void Stop()
        {
            this._timer.Stop();
            this.Elapsed = DateTimeOffset.Now - this._start;
        }
    }
}
