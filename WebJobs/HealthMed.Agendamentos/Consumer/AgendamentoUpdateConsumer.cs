using HealthMed.Application.Contracts;
using HealthMed.Domain.Entity;
using MassTransit;

namespace HealthMed.Agendamentos.Consumer
{
    public class AgendamentoUpdateConsumer(IAgendaServices agendaService) : IConsumer<AgendaEntity>
    {
        private readonly IAgendaServices _agendaService = agendaService;

        public async Task Consume(ConsumeContext<AgendaEntity> context)
        {
            await _agendaService.Update(context.Message!);
        }
    }
}
