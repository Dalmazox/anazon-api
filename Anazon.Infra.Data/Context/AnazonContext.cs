using Anazon.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Anazon.Infra.Data.Context
{
    public class AnazonContext : DbContext
    {
        public AnazonContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserMap).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
