namespace HealthMed.Data.DTO
{
    public class ReadAgendaDTO
    {
        public int IdAgenda { get; set; }
        public DateTime HorarioDisponivel { get; set; }
        public bool IsHorarioMarcado { get; set; }
        public bool IsMedicoNotificado { get; set; }
        public int IdMedico { get; set; }
        public int? IdPaciente { get; set; }
        public decimal ValorConsulta { get; set; }
        public ReadMedicoResumoDTO Medico { get; set; } // Apenas as informações necessárias do médico
        public ReadPacienteDTO Paciente { get; set; } // Apenas as informações necessárias do médico
    }

}
