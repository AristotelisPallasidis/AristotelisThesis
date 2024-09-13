using AristotelisThesis.WPF.State.Navigators;
using AristotelisThesis.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AristotelisThesis.WPF.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public readonly INavigator _navigator;

        public UpdateCurrentViewModelCommand(INavigator navigator)
        {
            _navigator = navigator;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;

                switch (viewType)
                {
                    case ViewType.Login:
                        _navigator.CurrentViewModel = new LoginViewModel();
                        break;
                    case ViewType.LoginWithFace:
                        _navigator.CurrentViewModel = new LoginWithFaceViewModel();
                        break;
                    case ViewType.LoginWithPalmprint:
                        _navigator.CurrentViewModel = new LoginWithPalmprintViewModel();
                        break;
                    case ViewType.Dashboard:
                        _navigator.CurrentViewModel = new DashboardViewModel();
                        break;
                    case ViewType.FaceRecognition:
                        _navigator.CurrentViewModel = new FaceRecognitionViewModel();
                        break;
                    case ViewType.PalmprintRecognition:
                        _navigator.CurrentViewModel = new PalmprintRecognitionViewModel();
                        break;
                    case ViewType.Statistics:
                        _navigator.CurrentViewModel = new StatisticsViewModel();
                        break;
                    case ViewType.Profile:
                        _navigator.CurrentViewModel = new ProfileViewModel();
                        break;
                    case ViewType.Settings:
                        _navigator.CurrentViewModel = new SettingsViewModel();
                        break;
                    //case ViewType.Register01ViewModel:
                    //    _navigator.CurrentViewModel = new Register01ViewModel();
                    //    break;
                    //case ViewType.Register02WithInformationViewModel:
                    //    _navigator.CurrentViewModel = new Register02WithInformationViewModel();
                    //    break;
                    //case ViewType.Register03InstructionsForPalmprintViewModel:
                    //    _navigator.CurrentViewModel = new Register03InstructionsForPalmprintViewModel();
                    //    break;
                    //case ViewType.Register04WithPalmprintViewModel:
                    //    _navigator.CurrentViewModel = new Register04WithPalmprintViewModel();
                    //    break;
                    //case ViewType.Register05InstructionsForFaceViewModel:
                    //    _navigator.CurrentViewModel = new Register05InstructionsForFaceViewModel();
                    //    break;
                    //case ViewType.Register06WithFaceViewModel:
                    //    _navigator.CurrentViewModel = new Register06WithFaceViewModel();
                    //    break;
                    default:
                        break;
                }
            }
        }
    }
}
   