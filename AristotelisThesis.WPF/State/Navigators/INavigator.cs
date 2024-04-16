using AristotelisThesis.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AristotelisThesis.WPF.State.Navigators
{
    public enum ViewType
    {
        Dashboard,
        FaceRecognition,
        PalmprintRecognition,
        Statistics,
        Profile,
        Settings,
        Login
    }

    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }
    }
}
