using System;
using System.Windows.Input;

namespace RemoteMergeUtility.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _EXECUTE;
        private readonly Func<bool>? _CAN_EXECUTE;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _EXECUTE = execute ?? throw new ArgumentNullException(nameof(execute));
            _CAN_EXECUTE = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _CAN_EXECUTE?.Invoke() ?? true;
        }

        public void Execute(object? parameter)
        {
            _EXECUTE();
        }
    }
}