using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AristotelisThesis.WPF.State.Accounts;
using AristotelisThesis.Domain.Models;

namespace AristotelisThesis.WPF.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly IAccountStore _accountStore;

        public ProfileViewModel(IAccountStore accountStore)
        {
            _accountStore = accountStore;
        }

        // ADD THE IMAGE OF THE CURRENT STUDENT (TAKE THE FIRST FROM THE FACE DATABASE)

        public int StudentAEM => _accountStore.CurrentAccount.AccountHolder.AEM;
        public string StudentName => _accountStore.CurrentAccount.AccountHolder.Name;
        public string StudentSurname => _accountStore.CurrentAccount.AccountHolder.Surname;
        public string StudentDepartment => "Τμήμα " + _accountStore.CurrentAccount.AccountHolder.Department;
        public string StudentSemester => $"{_accountStore.CurrentAccount.AccountHolder.Semester}ο εξάμηνο";
        public string StudentYearOfEntry => $"Έτος εισαγωγής {_accountStore.CurrentAccount.AccountHolder.YearOfEntry}";
        public string StudentAcademicEmail => _accountStore.CurrentAccount.AccountHolder.AcademicEmail;

    }
}
