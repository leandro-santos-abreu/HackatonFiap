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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração do relacionamento Médico -> Agenda (1:N)
        modelBuilder.Entity<AgendaEntity>()
            .HasOne(a => a.Medico)
            .WithMany(m => m.Agendas)
            .HasForeignKey(a => a.IdMedico)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração do relacionamento Paciente -> Agenda (1:1)
        modelBuilder.Entity<AgendaEntity>()
            .HasOne(a => a.Paciente)
            .WithMany(p => p.Agendamentos)
            .HasForeignKey(a => a.IdPaciente)
            .OnDelete(DeleteBehavior.SetNull); // Se o paciente for removido, a agenda continua livre
    }
}
