using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace RoleBasedAuthentication.Interfaces
{
    public interface IEmailWithAttachment : IEmailSender
    {
         Task SendEmailAsync(string email, string subject, string htmlMessage, List<Attachment> attachments);
    }
}
