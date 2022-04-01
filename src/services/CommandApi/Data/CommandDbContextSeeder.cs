using CommandApi.Business.Abstractions;
using CommandApi.GrpcDataServices.Abstractions;
using CommandApi.Models;

namespace CommandApi.Data
{
    public class CommandDbContextSeeder
    {
        private readonly IPlatformGrpcClient _grpcClient;
        private readonly ICommandService _commandService;

        public CommandDbContextSeeder(IPlatformGrpcClient grpcClient, ICommandService commandService)
        {
            _grpcClient = grpcClient;
            _commandService = commandService;
        }
        public async Task SeedAsync(CommandDbContext context, IWebHostEnvironment env, ILogger<CommandDbContextSeeder> logger, int? retry = 0)
        {
            int retryForAvaiability = 0;

            try
            {
                // if (!context.Platforms.Any())
                // {
                //     await context.Platforms.AddRangeAsync(await GetRemotePlatformsWithGrpc());

                //     await context.SaveChangesAsync();
                // }
                if (!context.Commands.Any())
                {
                    await context.Commands.AddRangeAsync(await GetDefaultCommands());

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
        private async Task<IEnumerable<Platform>> GetRemotePlatformsWithGrpc(){
            return await _grpcClient.ReturnAllPlatforms();
        }


        private async Task<IEnumerable<Command>> GetDefaultCommands()
        {

            // var platforms = await GetRemotePlatformsWithGrpc();

            // var kubernetesPlatformId = platforms.FirstOrDefault(x => x.Name == "Kubernetes").Id;

            return new List<Command>
            {
                new Command
                {
                    Id = Guid.NewGuid(),
                    Man = "return all deployments",
                    CommandLineName = "kubectl get deployments",
                    // PlatformId = kubernetesPlatformId
                },
                new Command
                {
                    Id =  Guid.NewGuid(),
                    Man = "return all services",
                    CommandLineName = "kubectl get services",
                    // PlatformId = kubernetesPlatformId
                },
                new Command
                {
                    Id= Guid.NewGuid(),
                    Man = "return all pods",
                    CommandLineName = "kubectl get pods",
                    // PlatformId = kubernetesPlatformId
                }
            };
        }
    }
}
