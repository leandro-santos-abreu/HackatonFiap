using HealthMed.Application.Contracts;
using HealthMed.Application.Services;
using HealthMed.Data;
using HealthMed.Notificacoes.NotificacoesConsumers;
using HealthMed.Persistence.Contract;
using HealthMed.Persistence.Repository;
using MassTransit;

var builder = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
{
    var configuration = hostContext.Configuration;

    var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
    var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
    var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;

    services.AddMassTransit(x =>
    {
        x.AddConsumer<EmailNotificationConsumer>();
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(new Uri(servidor), "/", h =>
            {
                h.Username(usuario);
                h.Password(senha);
            });

            cfg.ReceiveEndpoint("NotifyAgendamento", e =>
            {
                e.ConfigureConsumer<EmailNotificationConsumer>(context);
            });


            cfg.ConfigureEndpoints(context);
        });
    });

    services.AddScoped<HealthMedContext>();

    services.AddScoped<IAgendaRepository, AgendaRepository>();
    services.AddScoped<IAgendaServices, AgendaServices>();

});



var host = builder.Build();
host.Run();
