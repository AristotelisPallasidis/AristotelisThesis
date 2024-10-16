using AristotelisThesis.Domain.Exceptions;
using AristotelisThesis.Domain.Models;
using Microsoft.AspNet.Identity;

namespace AristotelisThesis.Domain.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountService _accountService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(IAccountService accountService, IPasswordHasher passwordHasher)
        {
            _accountService = accountService;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// This function is used to login a student to the system. It checks if the password is correct. If not, it throws an InvalidPasswordException.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="InvalidPasswordException"></exception>
        public async Task<Account> Login(string username, string password)
        {
            Account storedStudentAccount = await _accountService.GetByUsername(username);

            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(storedStudentAccount.AccountHolder.PasswordHash, password);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                throw new InvalidPasswordException(username, password);
            }

            return storedStudentAccount;
        }

        /// <summary>
        /// This function is used to register a new student to the database. It checks if the email and username are unique, and if the passwords match.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="sex"></param>
        /// <param name="phone"></param>
        /// <param name="address"></param>
        /// <param name="department"></param>
        /// <param name="semester"></param>
        /// <param name="aem"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="isPostgraduate"></param>
        /// <returns></returns>
        public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword, string name, string surname, string sex, string phone, string address, string department, int semester, int aem, DateTime dateOfBirth, int yearOfEntry, bool isPostgraduate)
        {
            RegistrationResult result = RegistrationResult.Success;

            if (password != confirmPassword)
            {
                result = RegistrationResult.PasswordsDoNotMatch;
            }

            Account emailAccount = await _accountService.GetByAcademicEmail(email);
            if (emailAccount != null)
            {
                result = RegistrationResult.EmailAlreadyExists;
            }

            Account usernameStudent= await _accountService.GetByUsername(username);
            if (usernameStudent != null)
            {
                result = RegistrationResult.UsernameAlreadyExists;
            }

            if (result == RegistrationResult.Success)
            {
                string hashedPassword = _passwordHasher.HashPassword(password);

                Student newStudent = new Student()
                {
                    AcademicEmail = email,
                    Username = username,
                    PasswordHash = hashedPassword,
                    Name = name,
                    Surname = surname,
                    Phone = phone,
                    Address = address,
                    Semester = semester,
                    Sex = sex,
                    AEM = aem,
                    DateOfBirth = dateOfBirth,
                    Department = department,
                    YearOfEntry = yearOfEntry,
                    IsPostgraduate = isPostgraduate,
                };

                Account newAccount = new Account()
                {
                    AccountHolder = newStudent
                };

                //await _studentService.Create(newStudent);
                await _accountService.Create(newAccount);
            }

            return result;
        }
    }
}
