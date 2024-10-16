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

        // Task<Student> GetByFace(string images);
        // Task<Student> GetByPalmprint(string images);
    }
}
