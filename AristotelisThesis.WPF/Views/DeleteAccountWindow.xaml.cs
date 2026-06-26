using System.Windows;

namespace AristotelisThesis.WPF.Windows
{
    /// <summary>
    /// Interaction logic for DeleteAccountWindow.xaml.
    /// Pure confirmation dialog: returns DialogResult true when the user confirms.
    /// </summary>
    public partial class DeleteAccountWindow : Window
    {
        public DeleteAccountWindow()
        {
            InitializeComponent();
        }

        // Setting DialogResult closes the dialog and returns to the caller.
        public void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
