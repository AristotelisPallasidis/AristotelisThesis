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
        /// <summary>
        /// Used when no connection string is configured, and by the EF design-time tools
        /// (`dotnet ef migrations add`), which construct this type with no arguments.
        /// </summary>
        public const string DefaultConnectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=AristotelisThesisDB;Trusted_Connection=True;";

        private readonly string _connectionString;

        /// <summary>Falls back to <see cref="DefaultConnectionString"/>; required by EF tooling.</summary>
        public AristotelisThesisDbContextFactory()
            : this(null)
        {
        }

        public AristotelisThesisDbContextFactory(string? connectionString)
        {
            _connectionString = string.IsNullOrWhiteSpace(connectionString)
                ? DefaultConnectionString
                : connectionString;
        }

        public AristotelisThesisDbContext CreateDbContext(string[]? args = null)
        {
            var options = new DbContextOptionsBuilder<AristotelisThesisDbContext>();
            options.UseSqlServer(_connectionString);

            return new AristotelisThesisDbContext(options.Options);
        }
    }
}
