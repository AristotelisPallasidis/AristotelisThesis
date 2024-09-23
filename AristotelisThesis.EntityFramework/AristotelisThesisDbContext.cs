using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AristotelisThesis.EntityFramework
{
    public class AristotelisThesisDbContext : DbContext
    {
        public AristotelisThesisDbContext(DbContextOptions options) : base(options) { }

        // Add to DbSet all the entities
        //public DbSet<Student> Students { get; set; }
    }
}
