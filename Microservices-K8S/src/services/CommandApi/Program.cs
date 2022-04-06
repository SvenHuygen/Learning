using System.Reflection;
using CommandApi.Business;
using CommandApi.Business.Abstractions;
using CommandApi.Data;
using CommandApi.GrpcDataServices;
using CommandApi.GrpcDataServices.Abstractions;
using CommandApi.MessageBus;
using CommandApi.MessageBus.EventProcessor;
using CommandApi.MessageBus.EventProcessor.Abstractions;
using Microsoft.EntityFrameworkCore;
using PlatformApi;
using Serilog;

string Namespace = Assembly.GetEntryAssembly().FullName;
string AppName = Namespace.Substring(0, Namespace.IndexOf(','));

var configuration = GetConfiguration();

Log.Logger = CreateSerilogLogger(configuration);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICommandService, CommandService>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddScoped<IPlatformGrpcClient, PlatformGrpcClient>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CommandDbContext>(options =>
{
    // if(builder.Environment.IsProduction()){
    //     options.UseSqlServer(configuration.GetConnectionString("command-mssql"));
    // } else {
    options.UseInMemoryDatabase("InMemoryPlatforms");
    //}
});

var app = builder.Build();


try
{
    using (var serviceScope = app.Services.CreateScope())
    {
        app.MigrateDbContext<CommandDbContext>((context, services) =>
            {
                var env = services.GetService<IWebHostEnvironment>();
                var logger = services.GetService<ILogger<CommandDbContextSeeder>>();

                new CommandDbContextSeeder(services.GetRequiredService<IPlatformGrpcClient>(), services.GetRequiredService<ICommandService>())
                .SeedAsync(context, env, logger)
                .Wait();
            }
        );
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
}
finally
{
    Log.CloseAndFlush();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    var seqServerUrl = configuration["Serilog:SeqServerUrl"];
    var logstashUrl = configuration["Serilog:LogstashgUrl"];
    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

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