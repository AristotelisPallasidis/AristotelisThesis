using System;
using System.ComponentModel;
using System.Text;

namespace AristotelisThesis.WPF.ViewModels
{
    /// <summary>
    /// This function is used to create a new ViewModel, it is used in the ViewModelFactoryRenavigator, to create a new ViewModel, when the ViewModel is navigated to.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <returns></returns>
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
