using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace RemoteMergeUtility.Models
{
	public class ProjectInfo : INotifyPropertyChanged
	{
		private string _Key = string.Empty;
		private string _Path = string.Empty;
		private readonly bool _IS_DEFAULT;

		public ProjectInfo() : this(false)
		{
		}

		public ProjectInfo(bool isDefault)
		{
			_IS_DEFAULT = isDefault;
		}

		[JsonConstructor]
		public ProjectInfo(string key, string path, bool isDefault)
		{
			_Key = key ?? string.Empty;
			_Path = path ?? string.Empty;
			_IS_DEFAULT = isDefault;
		}

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

		public bool IsDefault => _IS_DEFAULT;

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}