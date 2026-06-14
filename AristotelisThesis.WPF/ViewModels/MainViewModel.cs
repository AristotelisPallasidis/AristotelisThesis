using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Authenticators;
using AristotelisThesis.WPF.State.Navigators;
using AristotelisThesis.WPF.ViewModels.Factories;
using System.Windows.Input;

namespace AristotelisThesis.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IAristotelisThesisViewModelFactory _viewModelFactory;
        private readonly INavigator _navigator;
        private readonly IAuthenticator _authenticator;

        public bool IsLoggedIn => _authenticator.IsLoggedIn;
        public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

        /// <summary>
        /// Shared session start time. Lives on the shell view-model so the nav bar's
        /// duration counter is visible on every page while logged in.
        /// </summary>
        public System.DateTime? LoginTime => _authenticator.LoginTime;

        public ICommand UpdateCurrentViewModelCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainViewModel(INavigator navigator, IAristotelisThesisViewModelFactory viewModelFactory, IAuthenticator authenticator, IRenavigator loginRenavigator)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
            _authenticator = authenticator;

            _navigator.StateChanged += Navigator_StateChanged;
            _authenticator.StateChanged += Authenticator_StateChanged;

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, _viewModelFactory);
            LogoutCommand = new LogoutCommand(_authenticator, loginRenavigator);

            UpdateCurrentViewModelCommand.Execute(ViewType.Login);
        }

        private void Authenticator_StateChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(LoginTime));
        }

        private void Navigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
