using HealthMed.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
