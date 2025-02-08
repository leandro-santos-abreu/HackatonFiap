using HealthMed.Domain.Entity;

namespace HealthMed.Application.Contracts;
public interface IPacienteServices
{
    Task<IEnumerable<PacienteEntity>> Get();
    Task<PacienteEntity> GetById(int id);
   
    Task<bool> Create(PacienteEntity Paciente);

    bool Update(PacienteEntity updatedPaciente);

    Task<bool> Delete(int id);
}
