using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Navigators;
using System.Windows.Input;

namespace AristotelisThesis.WPF.ViewModels
{
    public class Register01ViewModel : ViewModelBase
    {

        public ICommand GoToViewLoginCommand { get; } // Back
        public ICommand GoToViewRegister02WithInformationCommand { get; } // Next

        public Register01ViewModel(IRenavigator loginRenavigator, IRenavigator register02Renavigator)
        {
            GoToViewLoginCommand = new RenavigateCommand(loginRenavigator);
            GoToViewRegister02WithInformationCommand = new RenavigateCommand(register02Renavigator);
        }

    }
}
