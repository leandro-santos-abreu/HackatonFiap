using HealthMed.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Data;
public class HealthMedContext : DbContext
{
    public HealthMedContext(DbContextOptions<HealthMedContext> options) : base(options)
    {            
    }
    public DbSet<MedicoEntity> Medico {  get; set; }
    public DbSet<PacienteEntity> Paciente {  get; set; }
    public DbSet<AgendaEntity> Agenda {  get; set; }
}
