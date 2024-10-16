using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services.AuthenticationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.WPF.State.Authenticators
{
    public interface IAuthenticator
    {
        Account CurrentAccount { get; }
        bool IsLoggedIn { get; }

        event Action StateChanged;

        Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword, string name, string surname, string sex, string phone, string address, string department, int semester, int aem, DateTime dateOfBirth, int yearOfEntry, bool isPostgraduate);
        Task<bool> Login(string username, string password);
        void Logout();
    }
}
