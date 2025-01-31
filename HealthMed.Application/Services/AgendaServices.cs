using HealthMed.Application.Contracts;
using HealthMed.Domain.Entity;
using HealthMed.Persistence.Contract;
using HealthMed.Persistence.Repository;


namespace HealthMed.Application.Services;
public class AgendaServices(IAgendaRepository medicoRepository) : IAgendaServices
{
    public async Task<bool> Create(AgendaEntity agenda)
    {
        var result = await medicoRepository.Create(agenda);
        return result;
    }

    public async Task<bool> Delete(int id)
    {
        var result = await medicoRepository.Delete(id);
        return result;
    }

    public async Task<IEnumerable<AgendaEntity>> Get()
    {
        var result = await medicoRepository.Get();
        return result;
    }   

    public async Task<AgendaEntity> GetById(int id)
    {
        var result = await medicoRepository.GetById(id);
        return result;
    }

    public bool Update(AgendaEntity updatedMedico)
    {
        var result = medicoRepository.Update(updatedMedico);
        return result;
    }
}
