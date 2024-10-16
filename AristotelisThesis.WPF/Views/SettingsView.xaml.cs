using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using AristotelisThesis.WPF.State.Accounts;
using AristotelisThesis.WPF.Windows;
using System.Windows;
using System.Windows.Controls;

namespace AristotelisThesis.WPF.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Window DeleteAccountWindow = new DeleteAccountWindow();
            DeleteAccountWindow.Owner = Application.Current.MainWindow;
            DeleteAccountWindow.ShowDialog();
            DeleteAccountWindow.Close();

            Console.WriteLine("Delete Account");
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Οι αλλαγές αποθηκεύτηκαν επιτυχώς!", "Αποθήκευση Αλλαγών", MessageBoxButton.OK, MessageBoxImage.Information);
            //_dataService.Update();
            //_accountService.Update(_accountStore.CurrentAccount.AccountHolder.Id, Account);
            Console.WriteLine("Save Changes");
        }
    }
}
