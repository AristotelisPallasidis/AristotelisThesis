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
using static System.Net.Mime.MediaTypeNames;

namespace AristotelisThesis.WPF.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();


            string name = "Αριστοτελία";
            string surname = "Παλλασίδου";
            //txtFullName.Text = $"{App.CurrentUser.FullName}";
            txtFullName.Text = $"{name} {surname}";


            //bool isUndergraduate = true;
            bool isUndergraduate = false;
            string userSex = "male";
            //string userSex = "female";

            if (isUndergraduate)
            {
                if (userSex == "male")
                {
                    txtStudentLevelandSex.Text = "Προπτυχιακός Φοιτητής -";
                }
                else
                {
                    txtStudentLevelandSex.Text = $"Προπτυχιακή Φοιτήτρια -";
                }
            }
            else
            {
                if (userSex == "male")
                {
                    txtStudentLevelandSex.Text = "Μεταπτυχιακός Φοιτητής -";
                }
                else
                {
                    txtStudentLevelandSex.Text = $"Μεταπτυχιακή Φοιτήτρια -";
                }
            }

            string department = "Πληροφορικής";
            txtDepartment.Text = $"Τμήμα {department}";

        }
    }
}
