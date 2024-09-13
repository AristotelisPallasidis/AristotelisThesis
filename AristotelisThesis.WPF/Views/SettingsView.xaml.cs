using AristotelisThesis.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            // load the user's data from the database
            inputName.Text = "Αριστοτέλης";
            inputSurname.Text = "Παλλασίδης";
            inputAddress.Text = "Αναγεννήσεως 19, Πευκα, Θεσσαλονίκη";
            inputEmail.Text = "arpalla@teiemt.gr";
            inputPhoneNumber.Text = "2101234567";
            inputDepartment.Text = "Πληροφορικής";
            inputYearOfEntry.Text = "2018";
            inputDateOfBirth.Text = "2000-10-30";
            inputAEM.Text = "4509";
            inputSemester.Text = "12";

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
            Console.WriteLine("Save Changes");
        }
    }
}
