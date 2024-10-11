using AristotelisThesis.Domain.Exceptions;
using AristotelisThesis.Domain.Models;
using Microsoft.AspNet.Identity;

namespace AristotelisThesis.Domain.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IStudentService _studentService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(IStudentService studentService, IPasswordHasher passwordHasher)
        {
            _studentService = studentService;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// This function is used to login a student to the system. It checks if the password is correct. If not, it throws an InvalidPasswordException.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="InvalidPasswordException"></exception>
        public async Task<Student> Login(string username, string password)
        {
            Student storedStudent = await _studentService.GetByUsername(username);

            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(storedStudent.PasswordHash, password);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                throw new InvalidPasswordException(username, password);
            }

            return storedStudent;

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
        public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword, string name, string surname, string sex, string phone, string address, string department, int semester, int aem, DateTime dateOfBirth, bool isPostgraduate)
        {
            RegistrationResult result = RegistrationResult.Success;

            if (password != confirmPassword)
            {
                result = RegistrationResult.PasswordsDoNotMatch;
            }

            Student emailAccount = await _studentService.GetByAcademicEmail(email);
            if (emailAccount != null)
            {
                result = RegistrationResult.EmailAlreadyExists;
            }

            Student usernameStudent= await _studentService.GetByUsername(username);
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
                    IsPostgraduate = isPostgraduate,

                };

                await _studentService.Create(newStudent);
            }

            return result;
        }
    }
}
