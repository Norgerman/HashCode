using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HashCode
{
    class BaseElement : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        public BaseElement() { }
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }
            else
            {
                storage = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
    }
}
