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
        private string _username { get; set; }
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand GoToViewLoginWithFaceCommand { get; }
        public ICommand GoToViewLoginWithPalmprintCommand { get; }
        public ICommand GoToViewRegister01Command { get; }


        public LoginViewModel(IAuthenticator authenticator, IRenavigator loginRenavigator, IRenavigator faceLoginRenavigator, IRenavigator palmprintLoginRenavigator, IRenavigator register01Renavigator)
        {
            LoginCommand = new LoginCommand(this, authenticator, loginRenavigator);
            GoToViewLoginWithFaceCommand = new RenavigateCommand(faceLoginRenavigator);
            GoToViewLoginWithPalmprintCommand = new RenavigateCommand(palmprintLoginRenavigator);
            GoToViewRegister01Command = new RenavigateCommand(register01Renavigator);
        }
    }

}
