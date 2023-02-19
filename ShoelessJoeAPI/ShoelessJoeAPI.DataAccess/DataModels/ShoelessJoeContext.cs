using Microsoft.EntityFrameworkCore;

namespace ShoelessJoeAPI.DataAccess.DataModels
{
    public class ShoelessJoeContext : DbContext
    {
        public ShoelessJoeContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(SecretConfig.ConnectionString, new MySqlServerVersion(SecretConfig.Version));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shoe>(enttiy =>
            {
                enttiy.Property(e => e.IsSold)
                .HasDefaultValue(false);
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<Manufacter> Manufacters { get; set; }
        public DbSet<Model> Models { get; set; }
    }
}
