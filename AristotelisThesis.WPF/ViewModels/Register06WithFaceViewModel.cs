using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Navigators;
using System.Windows.Input;

namespace AristotelisThesis.WPF.ViewModels
{
    public class Register06WithFaceViewModel : ViewModelBase
    {
        public ICommand GoToViewRegister05InstructionsForFaceCommand { get; } // Back
        public ICommand GoToViewDashboardCommand { get; } // Next

        public Register06WithFaceViewModel(IRenavigator register05Renavigator, IRenavigator dashboardRenavigator)
        {
            GoToViewRegister05InstructionsForFaceCommand = new RenavigateCommand(register05Renavigator);
            GoToViewDashboardCommand = new RenavigateCommand(dashboardRenavigator);
        }
    }
}
