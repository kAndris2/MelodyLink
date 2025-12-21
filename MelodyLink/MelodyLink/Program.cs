using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MelodyLink;
using MelodyLink.Models;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        Configure<LocalStorageSettings>(services, context);

        services.AddScoped<Application>();
    })
    .Build();

var app = host.Services.GetRequiredService<Application>();
app.Run();

static void Configure<T>(IServiceCollection services, HostBuilderContext context) 
    where T : class, new()
{
    services.Configure<T>(
        context.Configuration.GetSection(typeof(T).Name)
    );
}