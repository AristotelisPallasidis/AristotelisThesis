using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using AristotelisThesis.EntityFramework.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace AristotelisThesis.EntityFramework.Services
{
    public class StudentDataService : IStudentService
    {
        private readonly AristotelisThesisDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Student> _nonQueryDataService;

        public StudentDataService(AristotelisThesisDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Student>(contextFactory);
        }

        public async Task<Student> Create(Student entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public async Task<Student> Get(int id)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                Student entity = await context.Students
                    .FirstOrDefaultAsync(e => e.Id == id);
                
                return entity;
            }
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Student> entities = await 
                    context.Students.ToListAsync();

                return entities;
            }
        }

        public async Task<Student> GetByAcademicEmail(string email)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Students
                    .FirstOrDefaultAsync(a => a.AcademicEmail == email);
            }
        }

        public async Task<Student> GetByUsername(string username)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Students
                    .FirstOrDefaultAsync(a => a.Username == username);
            }
        }

        public async Task<Student> Update(int id, Student entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }
    }
}
