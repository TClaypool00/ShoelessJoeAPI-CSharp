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

        public DbSet<User> Users { get; set; }
    }
}
