using HealthMed.Data;
using HealthMed.Domain.Entity;
using HealthMed.Persistence.Contract;
using Microsoft.EntityFrameworkCore;


namespace HealthMed.Persistence.Repository;

public class AgendaRepository(HealthMedContext db) : IAgendaRepository
{
    public async Task<bool> Create(AgendaEntity agenda)
    {
        try
        {
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

    public async Task<IEnumerable<AgendaEntity>> Get() => await db.Agenda.ToListAsync();

    public async Task<AgendaEntity> GetById(int id)
    {
        var entity = await db.Agenda.FindAsync(id);
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
            db.Agenda.Update(updatedAgenda);
            db.SaveChanges();

            await Task.Delay(1);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
