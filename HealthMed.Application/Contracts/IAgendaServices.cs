using HealthMed.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Application.Contracts;
public interface IAgendaServices
{
    Task<IEnumerable<AgendaEntity>> Get();
    Task<AgendaEntity> GetById(int id);
   
    Task<bool> Create(AgendaEntity Agenda);

    bool Update(AgendaEntity updatedAgenda);

    Task<bool> Delete(int id);
}
