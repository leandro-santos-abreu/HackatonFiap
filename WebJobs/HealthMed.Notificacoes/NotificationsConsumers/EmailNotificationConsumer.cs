using HealthMed.Domain.Entity;
using MassTransit;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace HealthMed.Notificacoes.NotificacoesConsumers
{
    public class EmailNotificationConsumer(IConfiguration configuration) : IConsumer<AgendaEntity>
    {
        private IConfiguration _configuration = configuration;
        public async Task Consume(ConsumeContext<AgendaEntity> context)
        {
            var apiKey = _configuration.GetValue<string>("SendGrid:ApiKey");
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("leandro.sabreu@outlook.com", "HealthMed");
            var to = new EmailAddress($"{context.Message!.Medico?.Email}", $"{context.Message!.Medico?.Nome}");
            var subject = "Sending with SendGrid is Fun";

            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
