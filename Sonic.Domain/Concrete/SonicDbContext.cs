using Microsoft.EntityFrameworkCore;
using Sonic.Domain.Entities;

namespace Sonic.Domain.Concrete
{
    public class SonicDbContext : DbContext
    {
        public DbSet<Entities.System> Systems { get; set; }
        public DbSet<Role> Roles { get; set; }

        public SonicDbContext(DbContextOptions<SonicDbContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql(@"User ID=sonic;Password=Sonic1234!;Host=localhost;Port=5432;Database=sonic;Pooling=true;");
        //}
    }
}