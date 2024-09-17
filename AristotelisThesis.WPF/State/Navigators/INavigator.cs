using AristotelisThesis.WPF.ViewModels;
using System.Windows.Input;

namespace AristotelisThesis.WPF.State.Navigators
{
    public enum ViewType
    {
        Login,
        LoginWithFace,
        LoginWithPalmprint,
        Dashboard,
        FaceRecognition,
        PalmprintRecognition,
        Statistics,
        Profile,
        Settings,
        Register01ViewModel,
        Register02WithInformationViewModel,
        Register03InstructionsForPalmprintViewModel,
        Register04WithPalmprintViewModel,
        Register05InstructionsForFaceViewModel,
        Register06WithFaceViewModel,
    }

    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }
    }

}
