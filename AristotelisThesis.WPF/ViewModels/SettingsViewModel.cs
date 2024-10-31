using AristotelisThesis.WPF.State.Accounts;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.WPF.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IAccountStore _accountStore;

        public SettingsViewModel(IAccountStore accountStore)
        {
            _accountStore = accountStore;
        }

        // Personal Information Binding
        private string _studentName;
        public string StudentName
        {
            get => _studentName ?? _accountStore.CurrentAccount.AccountHolder.Name;
            set
            {
                _studentName = value;
                OnPropertyChanged(nameof(StudentName));
            }
        }

        private string _studentSurname;
        public string StudentSurname
        {
            get => _studentSurname ?? _accountStore.CurrentAccount.AccountHolder.Surname;
            set
            {
                _studentSurname = value;
                OnPropertyChanged(nameof(StudentSurname));
            }
        }

        private string _studentSex;
        public string StudentSex
        {
            get => _studentSex ?? _accountStore.CurrentAccount.AccountHolder.Sex;
            set
            {
                _studentSex = value;
                OnPropertyChanged(nameof(StudentSex));
            }
        }

        private string _studentAddress;
        public string StudentAddress
        {
            get => _studentAddress ?? _accountStore.CurrentAccount.AccountHolder.Address;
            set
            {
                _studentAddress = value;
                OnPropertyChanged(nameof(StudentAddress));
            }
        }

        private DateTime _studentDateOfBirth;
        public DateTime StudentDateOfBirth
        {
            get => _studentDateOfBirth != default ? _studentDateOfBirth : _accountStore.CurrentAccount.AccountHolder.DateOfBirth;
            set
            {
                _studentDateOfBirth = value;
                OnPropertyChanged(nameof(StudentDateOfBirth));
            }
        }

        private string _studentPhone;
        public string StudentPhone
        {
            get => _studentPhone ?? _accountStore.CurrentAccount.AccountHolder.Phone;
            set
            {
                _studentPhone = value;
                OnPropertyChanged(nameof(StudentPhone));
            }
        }

        // Academic Information Binding
        private string _studentAcademicEmail;
        public string StudentAcademicEmail
        {
            get => _studentAcademicEmail ?? _accountStore.CurrentAccount.AccountHolder.AcademicEmail;
            set
            {
                _studentAcademicEmail = value;
                OnPropertyChanged(nameof(StudentAcademicEmail));
            }
        }

        private int _studentAEM;
        public int StudentAEM
        {
            get => _studentAEM != default ? _studentAEM : _accountStore.CurrentAccount.AccountHolder.AEM;
            set
            {
                _studentAEM = value;
                OnPropertyChanged(nameof(StudentAEM));
            }
        }

        private string _studentDepartment;
        public string StudentDepartment
        {
            get => _studentDepartment ?? _accountStore.CurrentAccount.AccountHolder.Department;
            set
            {
                _studentDepartment = value;
                OnPropertyChanged(nameof(StudentDepartment));
            }
        }

        private int _studentYearOfEntry;
        public int StudentYearOfEntry
        {
            get => _studentYearOfEntry != default ? _studentYearOfEntry : _accountStore.CurrentAccount.AccountHolder.YearOfEntry;
            set
            {
                _studentYearOfEntry = value;
                OnPropertyChanged(nameof(StudentYearOfEntry));
            }
        }

        private int _studentSemester;
        public int StudentSemester
        {
            get => _studentSemester != default ? _studentSemester : _accountStore.CurrentAccount.AccountHolder.Semester;
            set
            {
                _studentSemester = value;
                OnPropertyChanged(nameof(StudentSemester));
            }
        }

        // Future Implementations
        // ICommand UpdateAccountDataCommand
        // ICommand DeleteAccountCommand

    }
}
