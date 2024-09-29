using AristotelisThesis.WPF.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace AristotelisThesis.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// This function is called when the application starts. It creates the main window and sets the view model.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            Window window = new MainWindow();
            window.DataContext = new MainViewModel();
            window.Show();

            base.OnStartup(e);
        }
    }

}
