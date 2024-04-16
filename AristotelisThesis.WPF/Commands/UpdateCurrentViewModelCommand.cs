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
        public event EventHandler? CanExecuteChanged;

        private readonly INavigator _navigator;

        public UpdateCurrentViewModelCommand(INavigator navigator)
        {
            _navigator = navigator;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;

                switch (viewType)
                {
                    case ViewType.Dashboard:
                        _navigator.CurrentViewModel = new DashboardViewModel();
                        break;
                    case ViewType.FaceRecognition:
                        _navigator.CurrentViewModel = new FaceRecognitionViewModel();
                        break;
                    case ViewType.PalmprintRecognition:
                        _navigator.CurrentViewModel = new PalmprintRecognitionViewModel();
                        break;
                    case ViewType.Profile:
                        _navigator.CurrentViewModel = new ProfileViewModel();
                        break;
                    case ViewType.Settings:
                        _navigator.CurrentViewModel = new SettingsViewModel();
                        break;
                    case ViewType.Statistics:
                        _navigator.CurrentViewModel = new StatisticsViewModel();
                        break;
                    default:
                        _navigator.CurrentViewModel = new DashboardViewModel();
                        break;
                }
            }
        }
    }
}
