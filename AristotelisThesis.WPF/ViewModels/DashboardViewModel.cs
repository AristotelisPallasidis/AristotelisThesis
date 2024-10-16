using AristotelisThesis.WPF.State.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.WPF.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IAccountStore _accountStore;

        public DashboardViewModel(IAccountStore accountStore)
        {
            _accountStore = accountStore;
        }

        public string StudentFullName => _accountStore.CurrentAccount.AccountHolder.Name + " " + _accountStore.CurrentAccount.AccountHolder.Surname;
        public string StudentDepartment => "Τμήμα " + _accountStore.CurrentAccount.AccountHolder.Department;

        public bool StudentIsPostgraduate => _accountStore.CurrentAccount.AccountHolder.IsPostgraduate;
        public string StudentSex => _accountStore.CurrentAccount.AccountHolder.Sex;


        public string StudentLevelAndGender
        {
            get
            {
                if (StudentSex == "Male" || StudentSex == "male")
                {
                    return StudentIsPostgraduate ? "Μεταπτυχιακός Φοιτητής -" : "Προπτυχιακός Φοιτητής -";
                }
                else if (StudentSex == "Female")
                {
                    return StudentIsPostgraduate ? "Μεταπτυχιακή Φοιτήτρια -" : "Προπτυχιακή Φοιτήτρια -";
                }

                return string.Empty; // Default case if no valid gender
            }
        }


    }
}
