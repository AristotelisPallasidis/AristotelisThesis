using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.Models;
using AristotelisThesis.WPF.ViewModels;
using System.ComponentModel;
using System.Windows.Input;

namespace AristotelisThesis.WPF.State.Navigators
{
    public class Navigator : ObservableObject, INavigator
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);

    }
}

// MOVED TO Models/ObservableObject.cs
//public event PropertyChangedEventHandler? PropertyChanged;
//protected void OnPropertyChanged(string proprertyName)
//{
//    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(proprertyName));
//}