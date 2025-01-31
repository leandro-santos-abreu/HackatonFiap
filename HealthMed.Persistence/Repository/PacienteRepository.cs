using HealthMed.Data;
using HealthMed.Domain.Entity;
using HealthMed.Persistence.Contract;
using Microsoft.EntityFrameworkCore;


namespace HealthMed.Persistence.Repository;

public class PacienteRepository(HealthMedContext db) : IPacienteRepository
{
    public async Task<bool> Create(PacienteEntity paciente)
    {
        try
        {
            await db.Paciente.AddAsync(paciente);
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
        var entity = db.Paciente.Find(id);
        if (entity == null)
        {
            return false;
        }
        db.Paciente.Remove(entity);

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<PacienteEntity>> Get() => await db.Paciente.ToListAsync();

    public async Task<PacienteEntity> GetById(int id)
    {
        var entity = await db.Paciente.FindAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Nenhum registro encontrado com o id: {id}");
        }

        return entity;
    }


    public bool Update(PacienteEntity updatedPaciente)
    {               
        try
        {
            db.Paciente.Update(updatedPaciente);
            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
