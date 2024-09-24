using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AristotelisThesis.EntityFramework.Services
{
    public class GenericDataService<T> : IDataService<T> where T : DomainObject
    {
        private readonly AristotelisThesisDbContextFactory _contextFactory;

        public GenericDataService(AristotelisThesisDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> Create(T entity)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<T> createdResult= await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                T? entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
                if (entity == null)
                {
                    return false;
                }
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }
        public async Task<T?> Get(int id)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                T? entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);

                return entity;
            }
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<T> entities = await context.Set<T>().ToListAsync();

                return entities;
            }
        }

        public async Task<T> Update(int id, T entity)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                entity.Id = id;

                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();

                return entity;
            }
        }

    }
}
