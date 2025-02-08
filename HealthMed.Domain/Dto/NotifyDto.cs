using HealthMed.Data.DTO;

namespace HealthMed.Domain.Dto
{
    public class NotifyDto
    {
        public required ReadAgendaDTO Agenda { get; set; }
        public required string Subject { get; set; }
        public required string Content { get; set; }
        public required string Receiver { get; set; }
    }
}
