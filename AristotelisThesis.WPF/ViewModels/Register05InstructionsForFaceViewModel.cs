using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Navigators;
using System.Windows.Input;

namespace AristotelisThesis.WPF.ViewModels
{
    public class Register05InstructionsForFaceViewModel : ViewModelBase
    {
        public ICommand GoToViewRegister04WithPalmprintCommand { get; } // Back
        public ICommand GoToViewRegister06WithFaceCommand { get; } // Next

        public Register05InstructionsForFaceViewModel(IRenavigator register04Renavigator, IRenavigator register06Renavigator)
        {
            GoToViewRegister04WithPalmprintCommand = new RenavigateCommand(register04Renavigator);
            GoToViewRegister06WithFaceCommand = new RenavigateCommand(register06Renavigator);
        }
    }
}
