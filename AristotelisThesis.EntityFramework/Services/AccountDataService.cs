using AristotelisThesis.Domain.Models;
using AristotelisThesis.Domain.Services;
using AristotelisThesis.EntityFramework.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.EntityFramework.Services
{
    public class AccountDataService : IAccountService
    {
        private readonly AristotelisThesisDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Account> _nonQueryDataService;

        public AccountDataService(AristotelisThesisDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Account>(contextFactory);
        }

        public async Task<Account> Create(Account entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            // Deleting an account removes its student too; the FK cascade then
            // cleans up the student's face images and session history.
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                Account account = await context.Accounts
                    .Include(a => a.AccountHolder)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (account == null)
                {
                    return false;
                }

                if (account.AccountHolder != null)
                {
                    context.Students.Remove(account.AccountHolder);
                }
                context.Accounts.Remove(account);

                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<Account> Get(int id)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                Account entity = await context.Accounts
                    .Include(a => a.AccountHolder)
                    .FirstOrDefaultAsync(e => e.Id == id);

                return entity;
            }
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Account> entities = await
                    context.Accounts.ToListAsync();

                return entities;
            }
        }

        public async Task<Account> GetByAcademicEmail(string email)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Accounts
                    .Include(a => a.AccountHolder)
                    .FirstOrDefaultAsync(a => a.AccountHolder.AcademicEmail == email);
            }
        }

        public async Task<Account> GetByStudentId(int studentId)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Accounts
                    .Include(a => a.AccountHolder)
                    .FirstOrDefaultAsync(a => a.AccountHolder.Id == studentId);
            }
        }

        public async Task<Account> GetByUsername(string username)
        {
            using (AristotelisThesisDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Accounts
                    .Include(a => a.AccountHolder)
                    .FirstOrDefaultAsync(a => a.AccountHolder.Username == username);
            }
        }

        public async Task<Account> Update(int id, Account entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }
    }
}
