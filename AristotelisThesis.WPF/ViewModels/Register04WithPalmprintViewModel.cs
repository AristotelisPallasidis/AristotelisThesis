using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Navigators;
using System.Windows.Input;

namespace AristotelisThesis.WPF.ViewModels
{
    public class Register04WithPalmprintViewModel : ViewModelBase
    {
        public ICommand GoToViewRegister03InstructionsForPalmprintCommand { get; } // Back
        public ICommand GoToViewRegister05InstructionsForFaceCommand { get; } // Next

        public Register04WithPalmprintViewModel(IRenavigator register03Renavigator, IRenavigator register05Renavigator)
        {
            GoToViewRegister03InstructionsForPalmprintCommand = new RenavigateCommand(register03Renavigator);
            GoToViewRegister05InstructionsForFaceCommand = new RenavigateCommand(register05Renavigator);
        }
    }
}
