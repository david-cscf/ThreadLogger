
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ThreadLogger;
using ThreadLoggerLibrary.Logic;
using ThreadLoggerLibrary.Utilities;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddScoped<IThreadManager, ThreadManager>();
builder.Services.AddScoped<IFileWriter, FileWriter>();
builder.Services.AddSingleton<App>();

IHostEnvironment env = builder.Environment;

builder.Configuration
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

using IHost host = builder.Build();
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

ILogger<Program>? log = null;
try
{
    log = services.GetRequiredService<ILogger<Program>>();
    services.GetRequiredService<App>().Run();
}
catch (Exception ex)
{	
	log?.LogError(ex, "Program execution failed");
}


