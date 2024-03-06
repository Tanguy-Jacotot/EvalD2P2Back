using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using EvalD2P2.Services;
using EvalD2P2.Services.Contracts;
using EvalD2P2.Repositories;
using EvalD2P2.Repositories.Contracts;


    
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IEventRepository, EventRepository>();

    })
    .Build();

await host.RunAsync();