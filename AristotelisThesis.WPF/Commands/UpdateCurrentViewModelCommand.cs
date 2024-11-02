using AristotelisThesis.WPF.State.Navigators;
using AristotelisThesis.WPF.ViewModels.Factories;
using System.Windows.Input;

namespace AristotelisThesis.WPF.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public readonly INavigator _navigator;
        private readonly IAristotelisThesisViewModelFactory _viewModelFactory;

        public UpdateCurrentViewModelCommand(INavigator navigator, IAristotelisThesisViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;

                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
            }
        }

    }
}
   