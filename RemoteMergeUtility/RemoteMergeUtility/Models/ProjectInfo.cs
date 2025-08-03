using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RemoteMergeUtility.Models
{
    public class ProjectInfo : INotifyPropertyChanged
    {
        private string _Key = string.Empty;
        private string _Path = string.Empty;

        public string Key
        {
            get => _Key;
            set
            {
                if (_Key != value)
                {
                    _Key = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Path
        {
            get => _Path;
            set
            {
                if (_Path != value)
                {
                    _Path = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}