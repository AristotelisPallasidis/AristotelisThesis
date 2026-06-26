using AristotelisThesis.WPF.ViewModels;
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

        private async void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var confirm = new DeleteAccountWindow { Owner = Application.Current.MainWindow };
            bool? confirmed = confirm.ShowDialog();

            if (confirmed != true)
            {
                return;
            }

            if (DataContext is SettingsViewModel vm)
            {
                bool ok = await vm.DeleteAccountAndLogout();
                if (!ok)
                {
                    MessageBox.Show("Η διαγραφή του λογαριασμού απέτυχε.", "Διαγραφή Λογαριασμού",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not SettingsViewModel vm)
            {
                return;
            }

            bool ok = await vm.SaveChanges();
            if (ok)
            {
                MessageBox.Show("Οι αλλαγές αποθηκεύτηκαν επιτυχώς!", "Αποθήκευση Αλλαγών",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Η αποθήκευση των αλλαγών απέτυχε.", "Αποθήκευση Αλλαγών",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
