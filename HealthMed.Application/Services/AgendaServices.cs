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

        var result = await agendaRepository.AgendarHorarioAsync(idPaciente, idAgenda);

        // 🚀 Aqui você pode enviar um e-mail para o médico notificando sobre o agendamento
        //if(result.Sucesso)
        //ToDo EnviarEmailParaMedico

        return result;
    }


    public async Task<ReadAgendaDTO> Create(AgendaEntity agenda)
    {
        try
        {
            var createdAgenda = await agendaRepository.Create(agenda);
            if (createdAgenda == null)
            {
                throw new Exception("Erro ao criar a agenda.");
            }

            // Retornar DTO da agenda recém-criada
            return new ReadAgendaDTO
            {
                IdAgenda = createdAgenda.IdAgenda,
                HorarioDisponivel = createdAgenda.HorarioDisponivel,
                IsHorarioMarcado = createdAgenda.isHorarioMarcado,
                IsMedicoNotificado = createdAgenda.isMedicoNotificado,
                IdMedico = createdAgenda.IdMedico,
                Medico = createdAgenda.Medico != null ? new ReadMedicoResumoDTO
                {
                    Nome = createdAgenda.Medico.Nome,
                    CRM = createdAgenda.Medico.CRM,
                    Especialidade = createdAgenda.Medico.Especialidade
                } : null
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
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

    public async Task<ReadAgendaDTO> UpdateAsync(AgendaEntity updatedMedico)
    {
        var updateAgenda = await agendaRepository.UpdateAsync(updatedMedico);
        if (updateAgenda == null)
        {
            throw new Exception("Erro ao criar a agenda.");
        }

        // Retornar DTO da agenda recém-criada
        return new ReadAgendaDTO
        {
            IdAgenda = updateAgenda.IdAgenda,
            HorarioDisponivel = updateAgenda.HorarioDisponivel,
            IsHorarioMarcado = updateAgenda.isHorarioMarcado,
            IsMedicoNotificado = updateAgenda.isMedicoNotificado,
            IdMedico = updateAgenda.IdMedico,
            Medico = updateAgenda.Medico != null ? new ReadMedicoResumoDTO
            {
                Nome = updateAgenda.Medico.Nome,
                CRM = updateAgenda.Medico.CRM,
                Especialidade = updateAgenda.Medico.Especialidade
            } : null
        };

    }

}
