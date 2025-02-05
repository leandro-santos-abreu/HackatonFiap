using HealthMed.Domain.Entity;

namespace HealthMed.Application.Contracts;
public interface IAgendaServices
{
    Task<IEnumerable<AgendaEntity>> Get();
    Task<AgendaEntity> GetById(int id);
   
    Task<bool> Create(AgendaEntity Agenda);

    Task<bool> Update(AgendaEntity updatedAgenda);

    Task<bool> Delete(int id);
}
