using HealthMed.Application.Contracts;
using HealthMed.Domain.Entity;
using MassTransit;
using MassTransit.Middleware;

namespace HealthMed.Agendamentos.Consumer
{
    public class AgendamentoCreateConsumer(IAgendaServices agendaService, IBus _bus) : IConsumer<AgendaEntity>
    {
        private readonly IAgendaServices _agendaService = agendaService;

        public async Task Consume(ConsumeContext<AgendaEntity> context)
        {
            await _agendaService.Create(context.Message!);

            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:NotifyAgendamento"));

            await endpoint.Send(context.Message!);
        }
    }
}
