using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Navigators;
using System.Windows.Input;

namespace AristotelisThesis.WPF.ViewModels
{
    public class Register03InstructionsForPalmprintViewModel : ViewModelBase
    {
        public ICommand GoToViewRegister02WithInformationCommand { get; } // Back
        public ICommand GoToViewRegister04WithPalmprintCommand { get; } // Next

        public Register03InstructionsForPalmprintViewModel(IRenavigator register02Renavigator, IRenavigator register04Renavigator)
        {
            GoToViewRegister02WithInformationCommand = new RenavigateCommand(register02Renavigator);
            GoToViewRegister04WithPalmprintCommand = new RenavigateCommand(register04Renavigator);
        }
    }
}
