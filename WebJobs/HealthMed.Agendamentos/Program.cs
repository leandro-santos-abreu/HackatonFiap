using HealthMed.Agendamentos.Consumer;
using HealthMed.Application.Contracts;
using HealthMed.Application.Services;
using HealthMed.Data;
using HealthMed.Persistence.Contract;
using HealthMed.Persistence.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
{
    var configuration = hostContext.Configuration;

    services.AddDbContext<HealthMedContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("HealthMedConnection")));

    var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
    var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
    var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;

    services.AddMassTransit(x =>
    {
        x.AddConsumer<AgendamentoCreateConsumer>();
        x.AddConsumer<AgendamentoUpdateConsumer>();
        x.AddConsumer<AgendamentoDeleteConsumer>();
        x.AddConsumer<AgendarHorarioCreateConsumer>();
        x.AddConsumer<CancelarAgendamentoCreateConsumer>();
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(new Uri(servidor), "/", h =>
            {
                h.Username(usuario);
                h.Password(senha);
            });

            cfg.ReceiveEndpoint("CreateAgendamento", e =>
            {
                e.ConfigureConsumer<AgendamentoCreateConsumer>(context);
            });

            cfg.ReceiveEndpoint("UpdateAgendamento", e =>
            {
                e.ConfigureConsumer<AgendamentoUpdateConsumer>(context);
            });

            cfg.ReceiveEndpoint("DeleteAgendamento", e =>
            {
                e.ConfigureConsumer<AgendamentoDeleteConsumer>(context);
            });
            
             cfg.ReceiveEndpoint("CreateAgendarHorario", e =>
             {
                 e.ConfigureConsumer<AgendarHorarioCreateConsumer>(context);
             });
            
            cfg.ReceiveEndpoint("CancelarAgendamento", e =>
             {
                 e.ConfigureConsumer<CancelarAgendamentoCreateConsumer>(context);
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
