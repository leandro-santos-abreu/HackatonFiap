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

    public class CancelarAgendamentoCreateConsumer(IAgendaServices agendaService) : IConsumer<CancelarAgendaRequestDTO>
    {
        private readonly IAgendaServices _agendaService = agendaService;

        public async Task Consume(ConsumeContext<CancelarAgendaRequestDTO> context)
        {
            var agendaMessage = context.Message;

            if (agendaMessage == null)
            {
                throw new ArgumentNullException(nameof(context.Message), "Mensagem recebida é nula.");
            }

            // Cancelar horário
            await _agendaService.CancelarAgendamento(agendaMessage.IdAgenda, agendaMessage.JustificativaCancelamento);

            //var agenda = await _agendaService.GetById(agendaMessage.IdAgenda);
            
            // Criar o DTO para notificação
            var notificationMessage = new CancelarAgendaRequestDTO
            {
                IdAgenda = agendaMessage.IdAgenda,
                JustificativaCancelamento = agendaMessage.JustificativaCancelamento
            };

            
        }
    }

}
