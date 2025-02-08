using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using MassTransit;
using DotNet.Testcontainers.Builders;
using System.Diagnostics;
using HealthMed.Presentation;
using HealthMed.Data;
using Microsoft.EntityFrameworkCore;
using HealthMed.Agendamentos.Consumer;
using HealthMed.Notificacoes.NotificationsConsumers;

namespace HealthMed.Tests.IntegrationTests.BaseClasses;

public class CustomWebApplicationFactory : WebApplicationFactory<Startup>, IAsyncDisposable
{
    private readonly MsSqlContainer _container;
    private readonly RabbitMqContainer _rabbitMqContainer;
    private readonly TaskCompletionSource<string> _connectionStringTcs = new TaskCompletionSource<string>();
    private IServiceProvider? _serviceProvider;
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.Test.json")
            .AddEnvironmentVariables()
            .Build();
    public CustomWebApplicationFactory()
    {
        _container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithAutoRemove(true)
            .Build();

        _rabbitMqContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:latest")
        .WithPortBinding(5672, 5672)
        .WithUsername("guest")
        .WithPassword("guest")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
        .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        if (!_configuration.GetValue<bool>("Settings:RunningCI"))
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var connectionString = _connectionStringTcs.Task.Result;
                config.AddInMemoryCollection(new[]
                {
                new KeyValuePair<string, string>("Settings:DbConnectionString", connectionString)
                });
            });


        builder.ConfigureServices(services =>
        {
            services.RemoveMassTransitHostedService();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<AgendarHorarioCreateConsumer>();
                x.AddConsumer<CancelarAgendamentoPacienteConsumer>();
                x.AddConsumer<ConfirmarAgendamentoConsumer>();
                x.AddConsumer<EmailNotificationConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(_configuration.GetValue<string>("MassTransit:Servidor")!), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("CreateAgendarHorario", e =>
                    {
                        e.ConfigureConsumer<AgendarHorarioCreateConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("CancelarAgendamento", e =>
                    {
                        e.ConfigureConsumer<CancelarAgendamentoPacienteConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("ConfirmarAgendamentoMedico", e =>
                    {
                        e.ConfigureConsumer<ConfirmarAgendamentoConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("Notify", e =>
                    {
                        e.ConfigureConsumer<EmailNotificationConsumer>(context);
                    });

                });
            });

            services.AddMassTransitHostedService();
        });
    }

    public async Task InitializeAsync()
    {
        if (!_configuration.GetValue<bool>("Settings:RunningCI"))
        {
            Console.WriteLine("Starting SQL Server container...");

            await _container.StartAsync();
            if (_container.State != DotNet.Testcontainers.Containers.TestcontainersStates.Running)
                throw new Exception("Failed to start  SQL Server container.");

            _connectionStringTcs.SetResult(_container.GetConnectionString());

            Console.WriteLine("SQL Server container started. Waiting for it to be ready...");

            Console.WriteLine("Starting RabbitMQ Container...");

            await _rabbitMqContainer.StartAsync();

            Console.WriteLine("RabbitMQ container started. Waiting for it to be ready...");

        }

        // Build service provider and apply migrations
        _serviceProvider = Services;

        using var scope = _serviceProvider.CreateScope();


        var migrator = scope.ServiceProvider.GetService<HealthMedContext>()
            ?? throw new InvalidOperationException("HealthMedContext service is not registered.");
        migrator.Database.Migrate();

        Console.WriteLine("Migrations Applied! SQL Server is ready.");
    }

    public override async ValueTask DisposeAsync()
    {
        if (!_configuration.GetValue<bool>("Settings:RunningCI"))
            return;

        await _container.StopAsync();
        await _container.DisposeAsync();

        await _rabbitMqContainer.StopAsync();
        await _rabbitMqContainer.DisposeAsync();
    }
}