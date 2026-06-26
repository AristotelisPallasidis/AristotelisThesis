using AristotelisThesis.Domain.Services;
using AristotelisThesis.WPF.State.Accounts;
using AristotelisThesis.WPF.State.Authenticators;
using AristotelisThesis.WPF.State.Navigators;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.WPF.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IAccountStore _accountStore;
        private readonly IAccountService _accountService;
        private readonly IAuthenticator _authenticator;
        private readonly ViewModelDelegateRenavigator<LoginViewModel> _loginRenavigator;

        public SettingsViewModel(IAccountStore accountStore, IAccountService accountService, IAuthenticator authenticator, ViewModelDelegateRenavigator<LoginViewModel> loginRenavigator)
        {
            _accountStore = accountStore;
            _accountService = accountService;
            _authenticator = authenticator;
            _loginRenavigator = loginRenavigator;
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
                OnPropertyChanged(nameof(IsMale));
                OnPropertyChanged(nameof(IsFemale));
            }
        }

        // Two-way bridges for the gender radio buttons (model stores "Male"/"Female").
        public bool IsMale
        {
            get => StudentSex == "Male";
            set { if (value) StudentSex = "Male"; }
        }

        public bool IsFemale
        {
            get => StudentSex == "Female";
            set { if (value) StudentSex = "Female"; }
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

        /// <summary>
        /// Persists the edited personal details to the current student record.
        /// Academic fields are read-only in the UI and left unchanged.
        /// </summary>
        public async Task<bool> SaveChanges()
        {
            var account = _accountStore.CurrentAccount;
            var student = account.AccountHolder;

            // Snapshot the fields we touch so a failed persist leaves the in-memory
            // model exactly as it was, rather than showing unsaved values as saved.
            var original = (student.Name, student.Surname, student.Sex,
                            student.Address, student.Phone, student.DateOfBirth);

            student.Name = StudentName;
            student.Surname = StudentSurname;
            student.Sex = StudentSex;
            student.Address = StudentAddress;
            student.Phone = StudentPhone;
            student.DateOfBirth = StudentDateOfBirth;

            try
            {
                await _accountService.Update(account.Id, account);
                return true;
            }
            catch
            {
                (student.Name, student.Surname, student.Sex,
                 student.Address, student.Phone, student.DateOfBirth) = original;
                return false;
            }
        }

        /// <summary>
        /// Permanently deletes the current account, then logs out and returns to login.
        /// </summary>
        public async Task<bool> DeleteAccountAndLogout()
        {
            try
            {
                int accountId = _accountStore.CurrentAccount.Id;

                bool deleted = await _accountService.Delete(accountId);
                if (!deleted)
                {
                    return false;
                }

                _authenticator.Logout();
                _loginRenavigator.Renavigate();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
