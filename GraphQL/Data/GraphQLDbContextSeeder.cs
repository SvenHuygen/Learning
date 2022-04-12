using GraphQL.Models;

namespace GraphQL.Data
{
    public class GraphQLDbContextSeeder
    {
        public async Task SeedAsync(GraphQLDbContext context, IWebHostEnvironment env, ILogger<GraphQLDbContextSeeder> logger, int? retry = 0)
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

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(GraphQLDbContext));

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
                    Name = "Podman",
                    LicenseKey = "LicenseKey1"
                },
                new Platform
                {
                    Id =  Guid.NewGuid(),
                    Name = "PostgreSQL",
                    LicenseKey = "LicenseKey2"
                },
                new Platform
                {
                    Id= Guid.NewGuid(),
                    Name= "Manjaro",
                    LicenseKey = "LicenseKey3"
                }
            };
        }
    }
}