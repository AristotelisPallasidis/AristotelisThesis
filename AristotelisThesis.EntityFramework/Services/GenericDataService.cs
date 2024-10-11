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

        /// <summary>
        /// This function creates a new entity in the database, and returns the created entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> Create(T entity)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<T> createdResult= await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }

        /// <summary>
        /// This function deletes an entity from the database, and returns true if the entity was deleted successfully.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This function returns an entity from the database, given its id.
        /// </summary>
        /// <param name="id"> Student User ID </param>
        /// <returns> The entity of the given Student Id </returns>
        public async Task<T?> Get(int id)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                T? entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);

                return entity;
            }
        }

        /// <summary>
        /// This function returns all entities of type T from the database. 
        /// </summary>
        /// <returns> All the entities </returns>
        public async Task<IEnumerable<T>> GetAll()
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<T> entities = await context.Set<T>().ToListAsync();

                return entities;
            }
        }

        /// <summary>
        /// This function updates an entity in the database, and returns the updated entity.
        /// </summary>
        /// <param name="id"> Students' User ID </param>
        /// <param name="entity"> Stundet as Entity </param>
        /// <returns> The Entity </returns>
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
