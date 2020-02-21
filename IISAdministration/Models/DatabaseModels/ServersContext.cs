using Microsoft.EntityFrameworkCore;

namespace IISAdministration.Models.DatabaseModels {

    public class ServersContext : DbContext {

        public DbSet<Server> Servers { get; set; }

        public DbSet<Metrics> Metrics { get; set; }

        public ServersContext(DbContextOptions<ServersContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Server>()
                .Property(s => s.Url)
                .IsRequired();
        }
    }
}
