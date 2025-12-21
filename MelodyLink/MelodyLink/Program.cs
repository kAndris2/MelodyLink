using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MelodyLink;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<Application>();
    })
    .Build();

var app = host.Services.GetRequiredService<Application>();
app.Run();