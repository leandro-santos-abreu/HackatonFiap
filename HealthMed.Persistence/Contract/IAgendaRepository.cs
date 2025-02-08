using HealthMed.Data.DTO;
using HealthMed.Domain.Entity;

namespace HealthMed.Persistence.Contract;

public interface IAgendaRepository
{
    Task<IEnumerable<ReadAgendaDTO>> Get();
    Task<AgendaEntity> GetById(int id);

    Task<AgendaEntity> Create(AgendaEntity Agenda);

    Task<AgendaEntity> UpdateAsync(AgendaEntity updatedAgenda);

    Task<bool> Delete(int id);

    Task<ResultadoAgendamentoDTO> AgendarHorarioAsync(int idPaciente, int idAgenda);

    Task<ResultadoAgendamentoDTO> ConfirmaAgendamento(int idAgenda, bool isAceiteAgendamento);

    Task<ResultadoAgendamentoDTO> CancelarAgendamentoAsync(int idAgenda, string JustificativaCancelamento);
}
