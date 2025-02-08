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

            db.Agenda.Update(agenda);
            await db.SaveChangesAsync();

            Task.Delay(1);

            return new ResultadoAgendamentoDTO(true, "Agendamento realizado com sucesso!");
        }
        catch (Exception ex)
        {
            return new ResultadoAgendamentoDTO(false, "Ocorreu um erro ao criar a agenda : " + ex.Message);
        }

    }

    public async Task<bool> ValidaAgendamentoExistenteAsync(AgendaEntity agenda)
    {
        return await db.Agenda
            .AnyAsync(a => a.IdMedico == agenda.IdMedico && a.HorarioDisponivel == agenda.HorarioDisponivel);
    }


    public async Task<AgendaEntity> Create(AgendaEntity agenda)
    {
        try
        {
            if (await ValidaAgendamentoExistenteAsync(agenda))
            {
                throw new InvalidOperationException("Esse horário já está marcado."); // Exceção com mensagem clara
            }

            await db.Agenda.AddAsync(agenda);
            await db.SaveChangesAsync(); // Tornar assíncrono para evitar deadlocks

            return await db.Agenda
                .Include(a => a.Medico)
                .FirstOrDefaultAsync(a => a.IdAgenda == agenda.IdAgenda);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao criar a agenda: {ex.Message}", ex); // Propagar erro com mais contexto
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
                CRM = a.Medico.CRM,
                Especialidade = a.Medico.Especialidade
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


    public async Task<AgendaEntity> UpdateAsync(AgendaEntity updatedAgenda)
    {

        if (await ValidaAgendamentoExistenteAsync(updatedAgenda))
        {
            throw new InvalidOperationException("Esse horário já está marcado.");
        }

        db.Agenda.Update(updatedAgenda);
        await db.SaveChangesAsync();

        return await db.Agenda
            .Include(a => a.Medico)
            .FirstOrDefaultAsync(a => a.IdAgenda == updatedAgenda.IdAgenda);

    }
}
