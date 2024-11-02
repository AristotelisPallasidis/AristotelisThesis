using AristotelisThesis.WPF.ViewModels;

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
        Register01,
        Register02WithInformation,
        Register03InstructionsForPalmprint,
        Register04WithPalmprint,
        Register05InstructionsForFace,
        Register06WithFace,
    }

    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        event Action StateChanged;
    }

}
