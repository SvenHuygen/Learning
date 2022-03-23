using CommandApi.Models;

namespace CommandApi.Data
{
    public class CommandDbContextSeeder
    {
        public async Task SeedAsync(CommandDbContext context, IWebHostEnvironment env, ILogger<CommandDbContextSeeder> logger, int? retry = 0)
        {
            int retryForAvaiability = 0;

            try
            {
                if (!context.Commands.Any())
                {
                    context.Commands.AddRange(GetDefaultCommands());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(CommandDbContext));

                    await SeedAsync(context, env, logger, retryForAvaiability);
                }
            }
        }

        private IEnumerable<Command> GetDefaultCommands()
        {
            return new List<Command>
            {
                new Command
                {
                    Id = Guid.NewGuid(),
                    Man = "return all deployments",
                    CommandLineName = "kubectl get deployments"
                },
                new Command
                {
                    Id =  Guid.NewGuid(),
                    Man = "return all services",
                    CommandLineName = "kubectl get services"
                },
                new Command
                {
                    Id= Guid.NewGuid(),
                    Man = "return all pods",
                    CommandLineName = "kubectl get pods"
                }
            };
        }
    }
}
