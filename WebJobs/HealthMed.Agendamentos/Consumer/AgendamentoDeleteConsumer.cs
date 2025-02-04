using HealthMed.Application.Contracts;
using HealthMed.Domain.Entity;
using MassTransit;

namespace HealthMed.Agendamentos.Consumer
{
    public class AgendamentoDeleteConsumer(IAgendaServices agendaService) : IConsumer<AgendaEntity>
    {
        private readonly IAgendaServices _agendaService = agendaService;

        public async Task Consume(ConsumeContext<AgendaEntity> context)
        {
            await _agendaService.Delete(context.Message!.IdAgenda);
        }
    }
}
