using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using AristotelisThesis.Domain.Services.AuthenticationServices;
using AristotelisThesis.WPF.State.Accounts;

namespace AristotelisThesis.WPF.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountStore _accountStore;
        private readonly ISessionTrackingService _sessionTrackingService;

        public Authenticator(IAuthenticationService authenticationService, IAccountStore accountStore, ISessionTrackingService sessionTrackingService)
        {
            _authenticationService = authenticationService;
            _accountStore = accountStore;
            _sessionTrackingService = sessionTrackingService;
        }

        public Account CurrentAccount
        {
            get
            {
                return _accountStore.CurrentAccount;
            }
            private set
            {
                _accountStore.CurrentAccount = value;
                StateChanged?.Invoke();
            }
        }

        /// <summary>
        /// This function is used to check if a student is logged in.
        /// </summary>
        public bool IsLoggedIn => CurrentAccount != null;

        /// <summary>
        /// The moment the current session started. Backed by the shared account store
        /// so it is the same value on every page while logged in.
        /// </summary>
        public DateTime? LoginTime => _accountStore.LoginTime;

        public event Action StateChanged;


        /// <summary>
        /// This function is used to login a student to the system.
        /// </summary>
        /// <param name="username"> Student's Username</param>
        /// <param name="password"> Student's Password</param>
        /// <returns></returns>
        public async Task<bool> Login(string username, string password)
        {
            bool success = true;

            try
            {
                Account account = await _authenticationService.Login(username, password);

                // Stamp the session start before exposing the account, so any page that
                // reacts to login already sees a consistent LoginTime.
                _accountStore.LoginTime = DateTime.Now;
                CurrentAccount = account;

                // Record the attendance check-in for the now logged-in student.
                if (CurrentAccount?.AccountHolder != null)
                {
                    await _sessionTrackingService.RecordCheckIn(CurrentAccount.AccountHolder.Id);
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// This function is used to logout a student from the system.
        /// </summary>
        public void Logout()
        {
            // Close the attendance session for the outgoing student before clearing state.
            int? studentId = CurrentAccount?.AccountHolder?.Id;
            if (studentId.HasValue)
            {
                // Run off the UI thread to avoid deadlocking on the async EF call.
                Task.Run(() => _sessionTrackingService.RecordCheckOut(studentId.Value)).GetAwaiter().GetResult();
            }

            _accountStore.LoginTime = null;
            CurrentAccount = null;
        }

        /// <summary>
        /// This function is used to register a student to the system. and return the result of the registration.
        /// </summary>
        public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword, string name, string surname, string sex, string phone, string address, string department, int semester, int aem, DateTime dateOfBirth, int yearOfEntry, bool isPostgraduate)
        {
            return await _authenticationService.Register(email, username, password, confirmPassword, name, surname, sex, phone, address, department, semester, aem, dateOfBirth, yearOfEntry, isPostgraduate);
        }
    }
}
