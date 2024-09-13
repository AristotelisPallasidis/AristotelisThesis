using System.Windows;

namespace AristotelisThesis.WPF.Windows
{
    /// <summary>
    /// Interaction logic for DeleteAccountWindow.xaml
    /// </summary>
    public partial class DeleteAccountWindow : Window
    {
        public DeleteAccountWindow()
        {
            InitializeComponent();
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog window
            this.Close();
        }


    }
}
