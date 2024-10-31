using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
