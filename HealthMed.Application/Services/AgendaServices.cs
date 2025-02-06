using HealthMed.Application.Contracts;
using HealthMed.Data.DTO;
using HealthMed.Domain.Entity;
using HealthMed.Persistence.Contract;
using HealthMed.Persistence.Repository;
using Microsoft.EntityFrameworkCore;


namespace HealthMed.Application.Services;
public class AgendaServices(IAgendaRepository agendaRepository) : IAgendaServices
{
    public async Task<ResultadoAgendamentoDTO> AgendarHorarioAsync(int idPaciente, int idAgenda)
    {  

       var result =  await agendaRepository.AgendarHorarioAsync(idPaciente, idAgenda);

        // 🚀 Aqui você pode enviar um e-mail para o médico notificando sobre o agendamento
        //if(result.Sucesso)
            //ToDo EnviarEmailParaMedico

        return result; 
    }


    public async Task<bool> Create(AgendaEntity agenda)
{
        var result = await agendaRepository.Create(agenda);
        return result;
    }

    public async Task<bool> Delete(int id)
    {
        var result = await agendaRepository.Delete(id);
        return result;
    }

    public async Task<IEnumerable<ReadAgendaDTO>> Get()
    {
        var result = await agendaRepository.Get();
        return result;
    }   

    public async Task<AgendaEntity> GetById(int id)
    {
        var result = await agendaRepository.GetById(id);
        return result;
    }

    public bool Update(AgendaEntity updatedMedico)
    {
        var result = agendaRepository.Update(updatedMedico);
        return result;
    }
    
}
