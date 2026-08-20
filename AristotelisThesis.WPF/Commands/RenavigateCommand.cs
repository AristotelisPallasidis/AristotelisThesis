using AristotelisThesis.WPF.State.Navigators;
using System.Windows.Input;

namespace AristotelisThesis.WPF.Commands
{
    public class RenavigateCommand : ICommand
    {
        private readonly IRenavigator _renavigator;

        public event EventHandler? CanExecuteChanged;

        public RenavigateCommand(IRenavigator renavigator)
        {
            _renavigator = renavigator;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
           _renavigator.Renavigate();
        }
    }
}
