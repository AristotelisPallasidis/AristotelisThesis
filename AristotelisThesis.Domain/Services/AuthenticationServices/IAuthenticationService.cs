using AristotelisThesis.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.Domain.Services.AuthenticationServices
{
    public enum RegistrationResult
    {
        Success,
        PasswordsDoNotMatch,
        EmailAlreadyExists,
        UsernameAlreadyExists
    }

    public interface IAuthenticationService
    {
        Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword, string name, string surname, string sex, string phone, string address, string department, int semester, int aem, DateTime dateOfBirth,int yearOfEntry, bool isPostgraduate);
        Task<Account> Login(string username, string password);

        /// <summary>
        /// Matches a probe face embedding against the enrolled embeddings and returns the
        /// owning <see cref="Account"/> when the best match is within the recognition threshold,
        /// or null when no enrolled face is close enough.
        /// </summary>
        Task<Account?> LoginWithFace(float[] probeEmbedding);

        /// <summary>
        /// Matches a probe palmprint embedding against the enrolled embeddings and returns the
        /// owning <see cref="Account"/> when the best match is within the recognition threshold,
        /// or null when no enrolled palm is close enough.
        /// </summary>
        Task<Account?> LoginWithPalmprint(float[] probeEmbedding);


    }
}
