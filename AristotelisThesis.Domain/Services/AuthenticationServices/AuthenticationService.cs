using AristotelisThesis.Domain.Exceptions;
using AristotelisThesis.Domain.Models;
using Microsoft.AspNet.Identity;

namespace AristotelisThesis.Domain.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        // dlib's recommended L2 distance threshold for a face match. Lower = stricter.
        // Max average L2 distance for a face match. dlib's 0.6 separates "same person vs.
        // random stranger" but is too loose for lookalikes/siblings, so we use a stricter 0.45.
        // Lower = stricter (fewer false accepts, more false rejects).
        private const double FaceMatchThreshold = 0.45;

        private readonly IAccountService _accountService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFaceImageService _faceImageService;

        public AuthenticationService(IAccountService accountService, IPasswordHasher passwordHasher, IFaceImageService faceImageService)
        {
            _accountService = accountService;
            _passwordHasher = passwordHasher;
            _faceImageService = faceImageService;
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
        /// Matches a probe face embedding against all enrolled embeddings using Euclidean
        /// distance and returns the owning account when the closest match is within the
        /// recognition threshold; otherwise returns null.
        /// </summary>
        public async Task<Account?> LoginWithFace(float[] probeEmbedding)
        {
            if (probeEmbedding == null || probeEmbedding.Length == 0)
            {
                return null;
            }

            IReadOnlyList<(int StudentId, float[] Embedding)> enrolled = await _faceImageService.GetAllEmbeddings();

            // Score each enrolled student by the AVERAGE distance to their photos (more robust
            // than the single closest photo, which lets a lookalike match on one lucky frame),
            // then take the best-scoring student.
            var ranked = enrolled
                .GroupBy(e => e.StudentId)
                .Select(g => new
                {
                    StudentId = g.Key,
                    Score = g.Average(e => EmbeddingSerializer.Distance(probeEmbedding, e.Embedding))
                })
                .OrderBy(x => x.Score)
                .ToList();

            if (ranked.Count == 0 || ranked[0].Score > FaceMatchThreshold)
            {
                return null;
            }

            return await _accountService.GetByStudentId(ranked[0].StudentId);
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
