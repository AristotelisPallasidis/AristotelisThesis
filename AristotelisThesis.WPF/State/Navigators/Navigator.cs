using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.ViewModels;
using AristotelisThesis.WPF.ViewModels.Factories;
using System.Windows.Input;

namespace AristotelisThesis.WPF.State.Navigators
{
    public class Navigator : INavigator
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;

    }
}
