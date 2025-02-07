using HealthMed.Data;
using HealthMed.Data.DTO;
using HealthMed.Domain.Entity;
using HealthMed.Persistence.Contract;
using Microsoft.EntityFrameworkCore;


namespace HealthMed.Persistence.Repository;

public class AgendaRepository(HealthMedContext db) : IAgendaRepository
{
    public async Task<ResultadoAgendamentoDTO> AgendarHorarioAsync(int idPaciente, int idAgenda)
    {
        try
        {
            var agenda = await db.Agenda
                .Include(a => a.Medico)
                .FirstOrDefaultAsync(a => a.IdAgenda == idAgenda);

            if (agenda == null)
                return new ResultadoAgendamentoDTO(false, "Agenda não encontrada.");

            if (agenda.isHorarioMarcado)
                return new ResultadoAgendamentoDTO(false, "Esse horário já está marcado.");

            var paciente = await db.Paciente.FindAsync(idPaciente);
            if (paciente == null)
                return new ResultadoAgendamentoDTO(false, "Paciente não encontrado.");

            // Atualizar a agenda com o paciente
            agenda.isHorarioMarcado = true;
            agenda.IdPaciente = idPaciente;
            agenda.Paciente = paciente;

            return new ResultadoAgendamentoDTO(true, "Agendamento realizado com sucesso!");
        }
        catch (Exception ex)
        {
            return new ResultadoAgendamentoDTO(false, "Ocorreu um erro ao criar a agenda : " + ex.Message);            
        }

    }

    public bool ValidaAgendamentoExistente(AgendaEntity agenda)
    {
        // Verifica se já existe uma agenda no mesmo horário para o médico
        bool agendaExistente = db.Agenda
            .Any(a => a.IdMedico == agenda.IdMedico && a.HorarioDisponivel == agenda.HorarioDisponivel);

        return agendaExistente;
    }

    public async Task<bool> Create(AgendaEntity agenda)
    {
        try
        {
            if (ValidaAgendamentoExistente(agenda))
            {
                throw new Exception() ;
            }

            await db.Agenda.AddAsync(agenda);
            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        var entity = db.Agenda.Find(id);
        if (entity == null)
        {
            return false;
        }
        db.Agenda.Remove(entity);

        await db.SaveChangesAsync();
        return true;
    }

    //public async Task<IEnumerable<AgendaEntity>> Get() => await db.Agenda.ToListAsync();

    public async Task<IEnumerable<ReadAgendaDTO>> Get()
    {
        var agendas = await db.Agenda
            .Include(a => a.Medico) // Carrega os dados do médico
            .ToListAsync();

        return agendas.Select(a => new ReadAgendaDTO
        {
            IdAgenda = a.IdAgenda,
            HorarioDisponivel = a.HorarioDisponivel,
            IsHorarioMarcado = a.isHorarioMarcado,
            IsMedicoNotificado = a.isMedicoNotificado,
            IdMedico = a.IdMedico,
            Medico = a.Medico != null ? new ReadMedicoResumoDTO
            {
                Nome = a.Medico.Nome,
                CRM = a.Medico.CRM
            } : null
        }).ToList();
    }

    public async Task<AgendaEntity> GetById(int id)
    {
        var entity = await db.Agenda
            .Include(a => a.Medico) // Carrega o médico associado
            .FirstOrDefaultAsync(a => a.IdAgenda == id); ;
        if (entity == null)
        {
            throw new KeyNotFoundException($"Nenhum registro encontrado com o id: {id}");
        }

        return entity;
    }


    public async Task<bool> Update(AgendaEntity updatedAgenda)
    {               
        try
        {
            if (ValidaAgendamentoExistente(updatedAgenda))
            {
                throw new Exception();
            }

            db.Agenda.Update(updatedAgenda);
            await db.SaveChangesAsync();

            Task.Delay(1);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
