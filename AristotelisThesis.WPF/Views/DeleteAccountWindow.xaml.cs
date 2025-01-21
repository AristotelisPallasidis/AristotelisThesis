using AristotelisThesis.Domain.Services;
using System.Windows;

namespace AristotelisThesis.WPF.Windows
{
    /// <summary>
    /// Interaction logic for DeleteAccountWindow.xaml
    /// </summary>
    public partial class DeleteAccountWindow : Window
    {
        IAccountService _accountService;
     
        public DeleteAccountWindow()
        {
            InitializeComponent();
        }

        public void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog window
            this.Close();
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog window
            this.Close();
        }


    }
}
