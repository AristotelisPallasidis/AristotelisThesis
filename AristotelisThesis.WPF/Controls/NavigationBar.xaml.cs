using AristotelisThesis.WPF.Commands;
using AristotelisThesis.WPF.State.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AristotelisThesis.WPF.Controls
{
    /// <summary>
    /// Interaction logic for NavigationBar.xaml
    /// </summary>
    public partial class NavigationBar : UserControl
    {
        private readonly IAuthenticator _authenticator;

        public NavigationBar()
        {
            InitializeComponent();
        }

        public NavigationBar(IAuthenticator authenticator)
        {
            _authenticator = authenticator;
        }

        //private void Logout_Click(object sender, RoutedEventArgs e)
        //{
        //    Application.Current.Shutdown();
        //    // TODO: Implement logout functionality
        //    //_authenticator.Logout();//
        //}
    }
}
