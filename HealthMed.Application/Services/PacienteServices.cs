using HealthMed.Application.Contracts;
using HealthMed.Domain.Entity;
using HealthMed.Persistence.Contract;
using HealthMed.Persistence.Repository;


namespace HealthMed.Application.Services;
public class PacienteServices(IPacienteRepository medicoRepository) : IPacienteServices
{
    public async Task<bool> Create(PacienteEntity paciente)
    {
        var result = await medicoRepository.Create(paciente);
        return result;
    }

    public async Task<bool> Delete(int id)
    {
        var result = await medicoRepository.Delete(id);
        return result;
    }

    public async Task<IEnumerable<PacienteEntity>> Get()
    {
        var result = await medicoRepository.Get();
        return result;
    }   

    public async Task<PacienteEntity> GetById(int id)
    {
        var result = await medicoRepository.GetById(id);
        return result;
    }

    public bool Update(PacienteEntity updatedMedico)
    {
        var result = medicoRepository.Update(updatedMedico);
        return result;
    }
}
