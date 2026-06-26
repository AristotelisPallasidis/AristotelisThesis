using AristotelisThesis.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.Domain.Services
{
    public interface IAccountService : IDataService<Account>
    {
        Task<Account> GetByUsername(string username);
        Task<Account> GetByAcademicEmail(string academicEmail);

        /// <summary>
        /// Returns the account whose holder is the given student, or null if none.
        /// Used to resolve a face-recognition match (which yields a student id) into an account.
        /// </summary>
        Task<Account> GetByStudentId(int studentId);

        // Task<Student> GetByPalmprint(string images);
    }
}
