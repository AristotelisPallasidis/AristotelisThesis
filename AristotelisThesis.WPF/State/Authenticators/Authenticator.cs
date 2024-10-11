using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services.AuthenticationServices;
using AristotelisThesis.WPF.Models;

namespace AristotelisThesis.WPF.State.Authenticators
{
    public class Authenticator : ObservableObject, IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;

        public Authenticator(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        private Student _currentStudent;
        public Student CurrentStudent
        {
            get
            {
                return _currentStudent;
            }
            private set
            {
                _currentStudent = value;
                OnPropertyChanged(nameof(CurrentStudent));
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public bool IsLoggedIn => CurrentStudent != null;

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
                CurrentStudent = await _authenticationService.Login(username, password);
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
            CurrentStudent = null;
        }

        /// <summary>
        /// This function is used to register a student to the system. and return the result of the registration.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword, string name, string surname, string sex, string phone, string address, string department, int semester, int aem, DateTime dateOfBirth, bool isPostgraduate)
        {
            return await _authenticationService.Register(email, username, password, confirmPassword, name, surname, sex, phone, address, department, semester, aem, dateOfBirth, isPostgraduate);
        }
    }
}
