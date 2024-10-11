using AristotelisThesis.WPF.State.Navigators;

namespace AristotelisThesis.WPF.ViewModels.Factories
{
    public interface IAristotelisThesisViewModelFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
