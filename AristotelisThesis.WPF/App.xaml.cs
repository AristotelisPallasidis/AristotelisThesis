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
        protected override void OnStartup(StartupEventArgs e)
        {
            Window window = new MainWindow();
            window.DataContext = new MainViewModel();
            window.Show();

            base.OnStartup(e);
        }
    }

}
