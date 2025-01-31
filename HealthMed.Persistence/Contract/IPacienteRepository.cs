using HealthMed.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Persistence.Contract;

public interface IPacienteRepository
{
    Task<IEnumerable<PacienteEntity>> Get();
    Task<PacienteEntity> GetById(int id);
   
    Task<bool> Create(PacienteEntity Paciente);

    bool Update(PacienteEntity updatedPaciente);

    Task<bool> Delete(int id);
}
