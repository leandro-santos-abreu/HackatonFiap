using HealthMed.Domain.Entity;

namespace HealthMed.Persistence.Contract;

public interface IPacienteRepository
{
    Task<IEnumerable<PacienteEntity>> Get();
    Task<PacienteEntity> GetById(int id);
   
    Task<bool> Create(PacienteEntity Paciente);

    bool Update(PacienteEntity updatedPaciente);

    Task<bool> Delete(int id);
}
