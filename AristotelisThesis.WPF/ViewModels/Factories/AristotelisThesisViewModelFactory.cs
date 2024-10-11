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
        // The CreateViewModel delegate is used to create the view models.
        private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
        //private readonly CreateViewModel<LoginWithFaceViewModel> _createLoginWithFaceViewModel;
        //private readonly CreateViewModel<LoginWithPalmprintViewModel> _createLoginWithPalmprintViewModel;
        //private readonly CreateViewModel<Register01ViewModel> _createRegister01ViewModel;
        //private readonly CreateViewModel<Register02WithInformationViewModel> _createRegister02WithInformationViewModel;
        //private readonly CreateViewModel<Register03InstructionsForPalmprintViewModel> _createRegister03InstructionsForPalmprintViewModel;
        //private readonly CreateViewModel<Register04WithPalmprintViewModel> _createRegister04WithPalmprintViewModel;
        //private readonly CreateViewModel<Register05InstructionsForFaceViewModel> _createRegister05InstructionsForFaceViewModel;
        //private readonly CreateViewModel<Register06WithFaceViewModel> _createRegister06WithFaceViewModel;
        private readonly CreateViewModel<DashboardViewModel> _createDashboardViewModel;
        private readonly CreateViewModel<FaceRecognitionViewModel> _createFaceRecognitionViewModel;
        private readonly CreateViewModel<PalmprintRecognitionViewModel> _createPalmprintRecognitionViewModel;
        private readonly CreateViewModel<ProfileViewModel> _createProfileViewModel;
        private readonly CreateViewModel<SettingsViewModel> _createSettingsViewModel;
        private readonly CreateViewModel<StatisticsViewModel> _createStatisticsViewModel;

        // The constructor takes in all the view models that are needed to be created.
        public AristotelisThesisViewModelFactory(
            CreateViewModel<LoginViewModel> createLoginViewModel,
            //CreateViewModel<LoginWithFaceViewModel> createLoginWithFaceViewModel,
            //CreateViewModel<LoginWithPalmprintViewModel> createLoginWithPalmprintViewModel,
            //CreateViewModel<Register01ViewModel> createRegister01ViewModel,
            //CreateViewModel<Register02WithInformationViewModel> createRegister02WithInformationViewModel,
            //CreateViewModel<Register03InstructionsForPalmprintViewModel> createRegister03InstructionsForPalmprintViewModel,
            //CreateViewModel<Register04WithPalmprintViewModel> createRegister04WithPalmprintViewModel,
            //CreateViewModel<Register05InstructionsForFaceViewModel> createRegister05InstructionsForFaceViewModel,
            //CreateViewModel<Register06WithFaceViewModel> createRegister06WithFaceViewModel,
            CreateViewModel<DashboardViewModel> createDashboardViewModel,
            CreateViewModel<FaceRecognitionViewModel> createFaceRecognitionViewModel,
            CreateViewModel<PalmprintRecognitionViewModel> createPalmprintRecognitionViewModel,
            CreateViewModel<ProfileViewModel> createProfileViewModel,
            CreateViewModel<SettingsViewModel> createSettingsViewModel,
            CreateViewModel<StatisticsViewModel> createStatisticsViewModel)
        {
            _createLoginViewModel = createLoginViewModel;
            //_createLoginWithFaceViewModel = createLoginWithFaceViewModel;
            //_createLoginWithPalmprintViewModel = createLoginWithPalmprintViewModel;
            //_createRegister01ViewModel = createRegister01ViewModel;
            //_createRegister02WithInformationViewModel = createRegister02WithInformationViewModel;
            //_createRegister03InstructionsForPalmprintViewModel = createRegister03InstructionsForPalmprintViewModel;
            //_createRegister04WithPalmprintViewModel = createRegister04WithPalmprintViewModel;
            //_createRegister05InstructionsForFaceViewModel = createRegister05InstructionsForFaceViewModel;
            //_createRegister06WithFaceViewModel = createRegister06WithFaceViewModel;
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
                //case ViewType.LoginWithFace:
                //    return _createLoginWithFaceViewModel();
                //case ViewType.LoginWithPalmprint:
                //    return _createLoginWithPalmprintViewModel();
                //case ViewType.Register01ViewModel:
                //    return _createRegister01ViewModel();
                //case ViewType.Register02WithInformationViewModel:
                //    return _createRegister02WithInformationViewModel();
                //case ViewType.Register03InstructionsForPalmprintViewModel:
                //    return _createRegister03InstructionsForPalmprintViewModel();
                //case ViewType.Register04WithPalmprintViewModel:
                //    return _createRegister04WithPalmprintViewModel();
                //case ViewType.Register05InstructionsForFaceViewModel:
                //    return _createRegister05InstructionsForFaceViewModel();
                //case ViewType.Register06WithFaceViewModel:
                //    return _createRegister06WithFaceViewModel();
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
