using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using RoleBasedAuthentication.Interfaces;
using RoleBasedAuthentication.Models;
using System.Net;
using System.Net.Mail;

namespace RoleBasedAuthentication.Services
{
    public class EmailSender : IEmailWithAttachment
    {
        private readonly EmailSettingsModal _emailSettings;
        public EmailSender(IOptions<EmailSettingsModal> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                    client.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.FromEmail),
                        Subject = subject,
                        Body = htmlMessage,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(email);

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex}");
            }
        }


        public async Task SendEmailAsync(string email, string subject, string htmlMessage, List<Attachment> attachment)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.To.Add(email);
                    message.Subject = subject;
                    message.Body = htmlMessage;
                    message.IsBodyHtml = true;
                    message.From = new MailAddress(_emailSettings.FromEmail);

                    foreach (var file in attachment)
                    {
                        message.Attachments.Add(file);
                    }

                    using (var client = new SmtpClient(_emailSettings.SmtpServer)) 
                    {
                        client.Port = 587; 
                        client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                        client.EnableSsl = true;
                        await client.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex}");
            }
        }
    }
}
