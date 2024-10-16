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


        // Task<Student> LoginWithFace(int[] image1, string password);
        // Task<Student> LoginWithPalmprint(string username, string password);

        
    }
}
