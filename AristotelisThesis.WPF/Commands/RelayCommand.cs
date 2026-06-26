using System;
using System.Windows.Input;

namespace AristotelisThesis.WPF.Commands
{
    /// <summary>
    /// A simple ICommand whose enabled state is driven by a <c>canExecute</c> predicate.
    /// CanExecuteChanged is hooked to <see cref="CommandManager.RequerySuggested"/>, so WPF
    /// re-evaluates it on UI input (e.g. as the user types), keeping bound buttons in sync.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _execute(parameter);
    }
}
