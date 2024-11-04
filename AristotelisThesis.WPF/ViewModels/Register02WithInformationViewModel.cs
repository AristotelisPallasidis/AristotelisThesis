using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Navigators;
using System.Windows.Input;

namespace AristotelisThesis.WPF.ViewModels
{
    public class Register02WithInformationViewModel : ViewModelBase
    {
        //private string _username;
        //public string Username
        //{
        //    get
        //    {
        //        return _username;
        //    }
        //    set
        //    {
        //        _username = value;
        //        //OnPropertyChange(nameof(Username));
        //    }
        //}

        public ICommand GoToViewRegister01Command { get; } // Back
        public ICommand GoToViewRegister03InstructionsForPalmprintCommand { get; } // Next

        public Register02WithInformationViewModel(IRenavigator register01Renavigator, IRenavigator register03Renavigator)
        {
            GoToViewRegister01Command = new RenavigateCommand(register01Renavigator);
            GoToViewRegister03InstructionsForPalmprintCommand = new RenavigateCommand(register03Renavigator);
        }

    }
}
