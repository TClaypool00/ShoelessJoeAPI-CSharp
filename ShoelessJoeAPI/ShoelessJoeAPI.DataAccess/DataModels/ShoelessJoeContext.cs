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
            modelBuilder.Entity<PotentialBuy>(enttiy =>
            {
                enttiy.Property(e => e.IsSold)
                .HasDefaultValue(false);
            });            

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.IsAdmin)
                .HasDefaultValue(false);
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<Manufacter> Manufacters { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PotentialBuy> PotentialBuys { get; set; }
        public DbSet<ShoeImage> ShoeImages { get; set; }
    }
}
