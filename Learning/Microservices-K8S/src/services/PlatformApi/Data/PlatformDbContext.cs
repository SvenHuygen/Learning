using Microsoft.EntityFrameworkCore;
using PlatformApi.Models;

namespace PlatformApi.Data
{
    public class PlatformDbContext : DbContext
    {
        public PlatformDbContext(DbContextOptions<PlatformDbContext> options) : base(options)
        {

        }

        public DbSet<Platform> Platforms { get; set; } 
    }
}
