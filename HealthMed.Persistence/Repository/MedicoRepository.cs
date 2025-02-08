using HealthMed.Data;
using HealthMed.Domain.Entity;
using HealthMed.Persistence.Contract;
using Microsoft.EntityFrameworkCore;


namespace HealthMed.Persistence.Repository;

public class MedicoRepository(HealthMedContext db) : IMedicoRepository
{
    public async Task<bool> Create(MedicoEntity medico)
    {
        try
        {
            await db.Medico.AddAsync(medico);
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
        var entity = db.Medico.Find(id);
        if (entity == null)
        {
            return false;
        }
        db.Medico.Remove(entity);

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<MedicoEntity>> Get() => await db.Medico.ToListAsync();

    public async Task<MedicoEntity> GetById(int id)
    {
        var entity = await db.Medico.FindAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Nenhum registro encontrado com o id: {id}");
        }

        return entity;
    }

    public async Task<MedicoEntity> GetByCRM(string CRM)
    {
        var entity = await db.Medico.FirstOrDefaultAsync(x => x.CRM.Equals(CRM));
        if (entity == null)
        {
            throw new KeyNotFoundException($"Nenhum registro encontrado com o CRM: {CRM}");
        }

        return entity;
    }

    public async Task<MedicoEntity> GetByNome(string Nome)
    {
        var entity = await db.Medico.FirstOrDefaultAsync(x => x.Nome.Contains(Nome));
        if (entity == null)
        {
            throw new KeyNotFoundException($"Nenhum registro encontrado com o Nome: {Nome}");
        }

        return entity;
    }

    public bool Update(MedicoEntity updatedMedico)
    {

        try
        {
            db.Medico.Update(updatedMedico);
            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<MedicoEntity> GetByEspecialidade(string Especialidade)
    {
        var entity = await db.Medico.FirstOrDefaultAsync(x => x.Especialidade.Contains(Especialidade));
        if (entity == null)
        {
            throw new KeyNotFoundException($"Nenhum registro encontrado com a Especialidade: {Especialidade}");
        }

        return entity;
    }
}
