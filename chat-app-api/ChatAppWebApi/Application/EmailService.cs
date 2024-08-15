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
        string Password = "Passwordforemailsender1!";
        int Port = 587;
        var client = new SmtpClient(MailServer, Port)
        {
            Credentials = new NetworkCredential(FromEmail, Password),
            EnableSsl = true,
        };
        MailMessage mailMessage = new MailMessage(FromEmail, toEmail, subject, body)
        {
            IsBodyHtml = isBodyHTML
        };

        return client.SendMailAsync(mailMessage);
    }

    private async Task SendConfirmationEmail(string? email, User? user, string token)
    {
    //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = $"http://localhost:3000/confirm-email?UserId={user.Id}&Token={token}";
        await this.SendEmailAsync(email, "Confirm Your Email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>;.", true);
    }
}
