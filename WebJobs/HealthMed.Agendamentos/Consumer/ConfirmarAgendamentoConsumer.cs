using AutoMapper;
using HealthMed.Application.Contracts;
using HealthMed.Data.DTO;
using HealthMed.Domain.Dto;
using MassTransit;

namespace HealthMed.Agendamentos.Consumer
{
    public class ConfirmarAgendamentoConsumer(IAgendaServices agendaService, IBus _bus, IMapper _mapper) : IConsumer<ConfirmarAgendaRequestDTO>
    {
        private readonly IAgendaServices _agendaService = agendaService;

        public async Task Consume(ConsumeContext<ConfirmarAgendaRequestDTO> context)
        {
            var agendaMessage = context.Message;

            if (agendaMessage == null)
            {
                throw new ArgumentNullException(nameof(context.Message), "Mensagem recebida é nula.");
            }

            // Cancelar horário
            await _agendaService.ConfirmaAgendamento(agendaMessage.IdAgenda, agendaMessage.AceitarAgendamento);

            var agenda = await _agendaService.GetById(agendaMessage.IdAgenda);

            NotifyDto notifyDto = new NotifyDto
            {
                Agenda = _mapper.Map<ReadAgendaDTO>(agenda),
                Subject = $"Agendamento {agenda.HorarioDisponivel:dd/MM/yyyy HH:m}",
                Content = $"O Agendamento {agenda.HorarioDisponivel:dd/MM/yyyy HH:m} foi {(agenda.isConfirmacaoMedico ? "confirmado" : "cancelado")} pelo médico",
                Receiver = "Paciente"

            };

            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:Notify"));

            await endpoint.Send(notifyDto);
        }
    }

}
