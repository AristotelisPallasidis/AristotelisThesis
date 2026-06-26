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

        /// <summary>The moment the current session started, or null when logged out.</summary>
        DateTime? LoginTime { get; }

        event Action StateChanged;

        Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword, string name, string surname, string sex, string phone, string address, string department, int semester, int aem, DateTime dateOfBirth, int yearOfEntry, bool isPostgraduate);
        Task<bool> Login(string username, string password);

        /// <summary>
        /// Logs in the student whose enrolled face matches the given probe embedding.
        /// Returns true on a successful match (account set, check-in recorded), false otherwise.
        /// </summary>
        Task<bool> LoginWithFace(float[] probeEmbedding);

        void Logout();
    }
}
