using PlatformApi.Models;

namespace PlatformApi.Data
{
    public class PlatformDbContextSeeder
    {
        public async Task SeedAsync(PlatformDbContext context, IWebHostEnvironment env, ILogger<PlatformDbContextSeeder> logger, int? retry = 0)
        {
            int retryForAvaiability = 0;

            try
            {
                if (!context.Platforms.Any())
                {
                    context.Platforms.AddRange(GetDefaultPlatforms());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(PlatformDbContext));

                    await SeedAsync(context, env, logger, retryForAvaiability);
                }
            }
        }

        private IEnumerable<Platform> GetDefaultPlatforms()
        {
            return new List<Platform>
            {
                new Platform
                {
                    Id = Guid.NewGuid(),
                    Name = "Dot Net",
                    Publisher = "Microsoft",
                    Cost = "Free"
                },
                new Platform
                {
                    Id =  Guid.NewGuid(),
                    Name = "SQL Server Express",
                    Publisher = "Microsoft",
                    Cost = "Free"
                },
                new Platform
                {
                    Id= Guid.NewGuid(),
                    Name= "Kubernetes",
                    Publisher = "Cloud Native Computing Foundation",
                    Cost = "Free"
                }
            };
        }
    }
}
