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
    public class Register01ViewModel : ViewModelBase
    {

        public ICommand GoToViewLoginCommand { get; }

        public Register01ViewModel(IRenavigator loginRenavigator)
        {
            GoToViewLoginCommand = new RenavigateCommand(loginRenavigator);
        }

    }
}
