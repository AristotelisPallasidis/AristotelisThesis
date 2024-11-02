using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Navigators;
using System.Windows.Input;

namespace AristotelisThesis.WPF.ViewModels
{
    public class LoginWithFaceViewModel : ViewModelBase
    {
        public ICommand GoToViewLoginCommand { get; }

        public LoginWithFaceViewModel(IRenavigator loginRenavigator)
        {
            GoToViewLoginCommand = new RenavigateCommand(loginRenavigator);
        }

    }
}
