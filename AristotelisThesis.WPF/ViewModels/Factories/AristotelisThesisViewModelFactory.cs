using AristotelisThesis.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.WPF.ViewModels.Factories
{
    public class AristotelisThesisViewModelFactory : IAristotelisThesisViewModelFactory
    {
        private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
        private readonly CreateViewModel<DashboardViewModel> _createDashboardViewModel;
        private readonly CreateViewModel<FaceRecognitionViewModel> _createFaceRecognitionViewModel;
        private readonly CreateViewModel<PalmprintRecognitionViewModel> _createPalmprintRecognitionViewModel;
        private readonly CreateViewModel<ProfileViewModel> _createProfileViewModel;
        private readonly CreateViewModel<SettingsViewModel> _createSettingsViewModel;
        private readonly CreateViewModel<StatisticsViewModel> _createStatisticsViewModel;

        public AristotelisThesisViewModelFactory(
            CreateViewModel<LoginViewModel> createLoginViewModel,
            CreateViewModel<DashboardViewModel> createDashboardViewModel,
            CreateViewModel<FaceRecognitionViewModel> createFaceRecognitionViewModel,
            CreateViewModel<PalmprintRecognitionViewModel> createPalmprintRecognitionViewModel,
            CreateViewModel<ProfileViewModel> createProfileViewModel,
            CreateViewModel<SettingsViewModel> createSettingsViewModel,
            CreateViewModel<StatisticsViewModel> createStatisticsViewModel)
        {
            _createLoginViewModel = createLoginViewModel;
            _createDashboardViewModel = createDashboardViewModel;
            _createFaceRecognitionViewModel = createFaceRecognitionViewModel;
            _createPalmprintRecognitionViewModel = createPalmprintRecognitionViewModel;
            _createProfileViewModel = createProfileViewModel;
            _createSettingsViewModel = createSettingsViewModel;
            _createStatisticsViewModel = createStatisticsViewModel;
        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {

            switch (viewType)
            {
                case ViewType.Login:
                    return _createLoginViewModel();
                case ViewType.Dashboard:
                    return _createDashboardViewModel();
                case ViewType.FaceRecognition:
                    return _createFaceRecognitionViewModel();
                case ViewType.PalmprintRecognition:
                    return _createPalmprintRecognitionViewModel();
                case ViewType.Profile:
                    return _createProfileViewModel();
                case ViewType.Settings:
                    return _createSettingsViewModel();
                case ViewType.Statistics:
                    return _createStatisticsViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }
    }
}
