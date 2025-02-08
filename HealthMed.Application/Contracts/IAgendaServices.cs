﻿using HealthMed.Data.DTO;
using HealthMed.Domain.Entity;

namespace HealthMed.Application.Contracts;
public interface IAgendaServices
{
    Task<IEnumerable<ReadAgendaDTO>> Get();
    Task<AgendaEntity> GetById(int id);

    Task<ReadAgendaDTO> Create(AgendaEntity Agenda);

    Task<ReadAgendaDTO> UpdateAsync(AgendaEntity updatedAgenda);

    Task<bool> Delete(int id);

    Task<ResultadoAgendamentoDTO> AgendarHorarioAsync(int idPaciente, int idAgenda);

    Task<ResultadoAgendamentoDTO> ConfirmaAgendamento(int idAgenda, bool isAceiteAgendamento);
    Task<ResultadoAgendamentoDTO> CancelarAgendamento(int idAgenda, string JustificativaCancelamento);
}
