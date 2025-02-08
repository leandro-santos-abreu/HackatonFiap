using MassTransit;
using SendGrid.Helpers.Mail;
using SendGrid;
using HealthMed.Domain.Dto;

namespace HealthMed.Notificacoes.NotificationsConsumers
{
    public class EmailNotificationConsumer(IConfiguration configuration) : IConsumer<NotifyDto>
    {
        private readonly IConfiguration _configuration = configuration;
        public async Task Consume(ConsumeContext<NotifyDto> context)
        {
            var apiKey = _configuration.GetValue<string>("SendGrid:ApiKey");
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("leandro.sabreu@outlook.com", "HealthMed");
            var to = new EmailAddress($"{context.Message!.Agenda.Medico?.Email}", $"{context.Message!.Agenda.Medico?.Nome}");


            if (context.Message.Receiver.Equals("Paciente"))
                to = new EmailAddress($"{context.Message!.Agenda.Paciente?.Email}", $"{context.Message!.Agenda.Paciente?.Nome}");

            var msg = MailHelper.CreateSingleEmail(from, to, context.Message!.Subject, context.Message.Content, context.Message.Content);
            await client.SendEmailAsync(msg);
        }
    }
}
