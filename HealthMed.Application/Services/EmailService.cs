using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using HealthMed.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace HealthMed.Application.Services;

public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient(configuration["Email:SmtpServer"])
            {
                Port = int.Parse(configuration["Email:Port"]),
                Credentials = new NetworkCredential(
                    configuration["Email:Username"],
                    configuration["Email:Password"]
                ),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(configuration["Email:Username"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);
            await smtpClient.SendMailAsync(mailMessage);

            logger.LogInformation($"E-mail enviado para {to} com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Erro ao enviar e-mail: {ex.Message}");
        }
    }
}
