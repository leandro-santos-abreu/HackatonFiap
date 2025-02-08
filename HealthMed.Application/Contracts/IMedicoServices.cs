using HealthMed.Domain.Entity;

namespace HealthMed.Application.Contracts;
public interface IMedicoServices
{
    Task<IEnumerable<MedicoEntity>> Get();
    Task<MedicoEntity> GetById(int id);
    Task<MedicoEntity> GetByNome(string Nome);
    Task<MedicoEntity> GetByCRM(string CRM);

    Task<MedicoEntity> GetByEspecialidade(string Especialidade);
    Task<bool> Create(MedicoEntity Medico);

    bool Update(MedicoEntity updatedMedico);

    Task<bool> Delete(int id);
}
