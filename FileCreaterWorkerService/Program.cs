using FileCreaterWorkerService;
using FileCreaterWorkerService.Services;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton(sp => new ConnectionFactory() { HostName = "localhost" });
        services.AddSingleton<RabbitMQSubscriber>();


    })
    .Build();

host.Run();
