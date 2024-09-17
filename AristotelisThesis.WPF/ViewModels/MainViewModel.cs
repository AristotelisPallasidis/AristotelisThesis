using AristotelisThesis.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public INavigator Navigator { get; set; } = new Navigator();

        public MainViewModel()
        {
            //Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Dashboard);

            Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Login);
            //Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.LoginWithFace);
            //Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.LoginWithPalmprint);


            //Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Register01ViewModel);
            //Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Register02WithInformationViewModel);
            //Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Register03InstructionsForPalmprintViewModel);
            //Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Register04WithPalmprintViewModel);
            //Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Register05InstructionsForFaceViewModel);
            //Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Register06WithFaceViewModel);


        }
    }
}
