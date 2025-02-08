using HealthMed.Application.Contracts;
using HealthMed.Domain.Entity;
using HealthMed.Persistence.Contract;
using HealthMed.Persistence.Repository;


namespace HealthMed.Application.Services;
public class MedicoServices(IMedicoRepository medicoRepository) : IMedicoServices
{
    public async Task<bool> Create(MedicoEntity medico)
    {
        try
        {            
            medico.Senha = BCrypt.Net.BCrypt.HashPassword(medico.Senha);

            return await medicoRepository.Create(medico);
        }
        catch (Exception ex)
        {
            return false;
        }
       
    }

    public async Task<bool> Delete(int id)
    {
        var result = await medicoRepository.Delete(id);
        return result;
    }

    public async Task<IEnumerable<MedicoEntity>> Get()
    {
        var result = await medicoRepository.Get();
        return result;
    }

    public async Task<MedicoEntity> GetByCRM(string CRM)
    {
        var result = await medicoRepository.GetByCRM(CRM);
        return result;
    }

    public async Task<MedicoEntity> GetById(int id)
    {
        var result = await medicoRepository.GetById(id);
        return result;
    }

    public async Task<MedicoEntity> GetByNome(string Nome)
    {
        var result = await medicoRepository.GetByNome(Nome);
        return result;
    }

    public bool Update(MedicoEntity updatedMedico)
    {
        try
        {
            updatedMedico.Senha = BCrypt.Net.BCrypt.HashPassword(updatedMedico.Senha);

            return medicoRepository.Update( updatedMedico);
        }
        catch (Exception ex)
        {
            return false;
        }
       
    }
}
