using System.Reflection;
using Serilog;
using GraphQL.Data;
using Microsoft.EntityFrameworkCore;
using GraphQL.GQL.Queries;
using GraphQL.Server.Ui.Voyager;
using GraphQL;

string Namespace = Assembly.GetEntryAssembly().FullName;
string AppName = Namespace.Substring(0, Namespace.IndexOf(','));

var configuration = GetConfiguration();

Log.Logger = CreateSerilogLogger(configuration);

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<GraphQLDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("postgresql"))
    );

builder.Services.AddGraphQLServer()
                .AddQueryType<Query>();

var app = builder.Build();

try
{
    app.MigrateDbContext<GraphQLDbContext>((context, services) =>
    {
        var env = services.GetService<IWebHostEnvironment>();
        var logger = services.GetService<ILogger<GraphQLDbContextSeeder>>();
        
        new GraphQLDbContextSeeder().SeedAsync(context, env, logger).Wait();
    });
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.UseGraphQLVoyager(new VoyagerOptions
{
    GraphQLEndPoint = "/graphql"
}, path: "/graphql-voyager");

app.Run();


IConfiguration GetConfiguration()
{
#if DEBUG
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
    return builder.Build();
#else
   var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
         return builder.Build();
#endif
}

Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}