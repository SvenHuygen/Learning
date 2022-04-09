using System.Reflection;
using Serilog;
using GraphQL.Data;
using Microsoft.EntityFrameworkCore;


string Namespace = Assembly.GetEntryAssembly().FullName;
string AppName = Namespace.Substring(0, Namespace.IndexOf(','));

var configuration = GetConfiguration();

Log.Logger = CreateSerilogLogger(configuration);

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<GraphQLDBContext>(options => 
    options.UseNpgsql(configuration.GetConnectionString("postgresql"))
    );

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

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