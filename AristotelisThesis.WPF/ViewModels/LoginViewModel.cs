using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Authenticators;
using AristotelisThesis.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AristotelisThesis.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand GoToViewLoginWithFaceCommand { get; }
        public ICommand GoToViewLoginWithPalmprintCommand { get; }
        public ICommand GoToViewRegister01Command { get; }


        public LoginViewModel(IRenavigator faceLoginRenavigator, IRenavigator palmprintLoginRenavigator, IRenavigator register01Renavigator)
        {
            GoToViewLoginWithFaceCommand = new RenavigateCommand(faceLoginRenavigator);
            GoToViewLoginWithPalmprintCommand = new RenavigateCommand(palmprintLoginRenavigator);
            GoToViewRegister01Command = new RenavigateCommand(register01Renavigator);
        }
    }

}
