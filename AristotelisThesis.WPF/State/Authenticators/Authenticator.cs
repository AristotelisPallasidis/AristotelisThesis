using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services.AuthenticationServices;
using AristotelisThesis.WPF.Models;
using AristotelisThesis.WPF.State.Accounts;

namespace AristotelisThesis.WPF.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountStore _accountStore;

        public Authenticator(IAuthenticationService authenticationService, IAccountStore accountStore)
        {
            _authenticationService = authenticationService;
            _accountStore = accountStore;
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
                CurrentAccount = await _authenticationService.Login(username, password);
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
            _accountStore.CurrentAccount.AccountHolder = null;
            //CurrentAccount = null;
        }

        /// <summary>
        /// This function is used to register a student to the system. and return the result of the registration.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword, string name, string surname, string sex, string phone, string address, string department, int semester, int aem, DateTime dateOfBirth, int yearOfEntry, bool isPostgraduate)
        {
            return await _authenticationService.Register(email, username, password, confirmPassword, name, surname, sex, phone, address, department, semester, aem, dateOfBirth, yearOfEntry, isPostgraduate);
        }
    
    }
}
