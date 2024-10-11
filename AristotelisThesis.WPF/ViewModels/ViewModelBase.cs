using AristotelisThesis.WPF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AristotelisThesis.WPF.ViewModels
{
    /// <summary>
    /// This function is used to create a new ViewModel, it is used in the ViewModelFactoryRenavigator, to create a new ViewModel, when the ViewModel is navigated to.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <returns></returns>
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;

    public class ViewModelBase : ObservableObject
    {
    }
}
