using AutoMapper;
using HealthMed.Application.Contracts;
using HealthMed.Data.DTO;
using HealthMed.Domain.Dto;
using MassTransit;

namespace HealthMed.Agendamentos.Consumer
{
    public class AgendarHorarioCreateConsumer(IAgendaServices agendaService, IBus _bus, IMapper _mapper) : IConsumer<AgendamentoRequestDTO>
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

            if (agenda.isMedicoNotificado)
                return;

            NotifyDto notifyDto = new NotifyDto
            {
                Agenda = _mapper.Map<ReadAgendaDTO>(agenda),
                Subject = "Agendamento Solicitado com Sucesso!",
                Content = "O Agendamento Foi Solicitado com Sucesso!",
                Receiver = "Medico"
            };

            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:Notify"));

            await endpoint.Send(notifyDto);

            agenda.isMedicoNotificado = true;
            await _agendaService.UpdateAsync(agenda);
        }
    }

}
