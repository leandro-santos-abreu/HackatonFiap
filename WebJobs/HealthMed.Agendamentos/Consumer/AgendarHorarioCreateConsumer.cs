using HealthMed.Application.Contracts;
using HealthMed.Data.DTO;
using HealthMed.Domain.Entity;
using MassTransit;
using MassTransit.Middleware;

namespace HealthMed.Agendamentos.Consumer
{
    //public class AgendarHorarioCreateConsumer(IAgendaServices agendaService, IBus _bus) : IConsumer<AgendaEntity>
    //{
    //    private readonly IAgendaServices _agendaService = agendaService;

    //    public async Task Consume(ConsumeContext<AgendaEntity> context)
    //    {
    //        await _agendaService.AgendarHorarioAsync(context.Message.IdPaciente,context.Message.IdAgenda);

    //        var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:NotifyAgendamento"));

    //        await endpoint.Send(context.Message!);
    //    }
    //}

    public class AgendarHorarioCreateConsumer(IAgendaServices agendaService, IBus _bus) : IConsumer<AgendamentoRequestDTO>
    {
        private readonly IAgendaServices _agendaService = agendaService;

        public async Task Consume(ConsumeContext<AgendamentoRequestDTO> context)
        {
            var agendaMessage = context.Message;

            if (agendaMessage == null)
            {
                throw new ArgumentNullException(nameof(context.Message), "Mensagem recebida é nula.");
            }

            // Agendar horário
            await _agendaService.AgendarHorarioAsync(agendaMessage.IdPaciente, agendaMessage.IdAgenda);

            var agenda = await _agendaService.GetById(agendaMessage.IdAgenda);
            
            // Criar o DTO para notificação
            var notificationMessage = new AgendamentoRequestDTO
            {
                IdAgenda = agendaMessage.IdAgenda,
                IdPaciente = agendaMessage.IdPaciente
            };

            // Enviar para a fila de notificação
            var endpoint = await _bus.GetSendEndpoint(new Uri("queue:NotifyAgendamento"));
            await endpoint.Send(agenda);
        }
    }

}
