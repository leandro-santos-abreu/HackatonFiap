using HealthMed.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Persistence.Contract;

public interface IMedicoRepository
{
    Task<IEnumerable<MedicoEntity>> Get();
    Task<MedicoEntity> GetById(int id);
    Task<MedicoEntity> GetByNome(string Nome);
    Task<MedicoEntity> GetByCRM(string CRM);
    Task<bool> Create(MedicoEntity Medico);

    bool Update(MedicoEntity updatedMedico);

    Task<bool> Delete(int id);
}
