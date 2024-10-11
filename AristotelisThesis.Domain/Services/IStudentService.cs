using AristotelisThesis.Domain.Models;

namespace AristotelisThesis.Domain.Services
{
    public interface IStudentService : IDataService<Student>
    {
        Task<Student> GetByUsername(string username);
        Task<Student> GetByAcademicEmail(string academicEmail);

        // Task<Student> GetByFace(string images);
        // Task<Student> GetByPalmprint(string images);
    }
}
