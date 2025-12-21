using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MelodyLink;
using MelodyLink.Models;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<LocalStorageSettings>(
            context.Configuration.GetSection("LocalStorageSettings")
        );

        services.AddScoped<Application>();
    })
    .Build();

var app = host.Services.GetRequiredService<Application>();
app.Run();