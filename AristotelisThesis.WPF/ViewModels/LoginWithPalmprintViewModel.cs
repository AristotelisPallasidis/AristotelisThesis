using System.Windows.Input;
using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Navigators;

namespace AristotelisThesis.WPF.ViewModels
{
    public class LoginWithPalmprintViewModel : ViewModelBase
    {
        public ICommand GoToViewLoginCommand { get; }

        public LoginWithPalmprintViewModel(IRenavigator loginRenavigator)
        {
            GoToViewLoginCommand = new RenavigateCommand(loginRenavigator);
        }

    }
}
