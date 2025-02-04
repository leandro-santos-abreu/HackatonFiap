using HealthMed.Application.Contracts;
using HealthMed.Domain.Entity;
using HealthMed.Persistence.Contract;
using HealthMed.Persistence.Repository;


namespace HealthMed.Application.Services;
public class PacienteServices(IPacienteRepository pacienteRepository) : IPacienteServices
{
    public async Task<bool> Create(PacienteEntity paciente)
    {
        try
        {
            paciente.Senha = BCrypt.Net.BCrypt.HashPassword(paciente.Senha);

            return await pacienteRepository.Create(paciente);
        }
        catch (Exception ex)
        {
            return false;
        }
        
    }

    public async Task<bool> Delete(int id)
    {
        var result = await pacienteRepository.Delete(id);
        return result;
    }

    public async Task<IEnumerable<PacienteEntity>> Get()
    {
        var result = await pacienteRepository.Get();
        return result;
    }   

    public async Task<PacienteEntity> GetById(int id)
    {
        var result = await pacienteRepository.GetById(id);
        return result;
    }

    public bool Update(PacienteEntity updatedPaciente)
    {
        try
        {
            updatedPaciente.Senha = BCrypt.Net.BCrypt.HashPassword(updatedPaciente.Senha);

            return pacienteRepository.Update(updatedPaciente);
        }
        catch (Exception ex)
        {
            return false;
        }       
    }
}
