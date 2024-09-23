using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.EntityFramework
{
    public class AristotelisThesisDbContextFactory : IDesignTimeDbContextFactory<AristotelisThesisDbContext>
    {
        public AristotelisThesisDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<AristotelisThesisDbContext>();
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AristotelisThesisDB;Trusted_Connection=True;");

            return new AristotelisThesisDbContext(options.Options);
        }
    }
}
