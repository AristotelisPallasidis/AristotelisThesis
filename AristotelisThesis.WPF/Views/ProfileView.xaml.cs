using AristotelisThesis.WPF.Commands;
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

namespace AristotelisThesis.WPF.Views
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            InitializeComponent();

            // Load an Image from the db that the user has registered with
            // Image = db.GetImage(); 

            // Fill the text with the user's data
            //txtName.Text = "Αριστοτέλης";
            //txtSurname.Text = "Παλλασίδης";
            //txtDepartment.Text = "Τμήμα " + "Πληροφορικής";
            //txtAEM.Text = "4509";
            //txtSemester.Text = "12" + "ο " + "Εξάμηνο";
            //txtYearOfEntry.Text = "Έτος Εισαγωγής " + "2018";
            //txtEmail.Text = "arpalla@teiemt.gr";
        }

    }
}
