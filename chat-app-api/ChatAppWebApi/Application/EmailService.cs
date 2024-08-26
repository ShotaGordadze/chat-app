using System.Net.Mail;
using System.Net;
using Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application;

public interface IEmailService
{
    Task SendEmailAsync(string toMail, string subject, string body, bool isBodyHtml);
}

public class EmailService : IEmailService
{
    public Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHTML)
    {
        string MailServer = "smtp.gmail.com";
        string FromEmail = "shotasemailsender@gmail.com";
        string Password = "fzvb dppx ybiu ajze";
        int Port = 587;

        var client = new SmtpClient(MailServer, Port)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(FromEmail, Password),
            EnableSsl = true,
        };

        MailMessage mailMessage = new MailMessage(FromEmail, toEmail, subject, body)
        {
            IsBodyHtml = isBodyHTML
        };

        return client.SendMailAsync(mailMessage);
    }
}
