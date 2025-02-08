using AutoMapper;
using HealthMed.Application.Contracts;
using HealthMed.Data.DTO;
using HealthMed.Domain.Dto;
using MassTransit;

namespace HealthMed.Agendamentos.Consumer
{
    public class CancelarAgendamentoPacienteConsumer(IAgendaServices agendaService, IBus _bus, IMapper _mapper) : IConsumer<CancelarAgendaRequestDTO>
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

            var agenda = await _agendaService.GetById(agendaMessage.IdAgenda);

            NotifyDto notifyDto = new NotifyDto
            {
                Agenda = _mapper.Map<ReadAgendaDTO>(agenda),
                Subject = "Agendamento Cancelado com Sucesso!",
                Content = $"O Agendamento Foi Cancelado Pelo Paciente por Conta da Seguinte Justificativa: {agenda.JustificativaCancelamento}",
                Receiver = "Medico"
            };

            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:Notify"));

            await endpoint.Send(notifyDto);
        }
    }

}
