using Ticket_Hub.Models.Models;

namespace Ticket_Hub.Services.IServices;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string toEmail, string subject, string body);
    Task<bool> SendEmailToClientAsync(string toEmail, string subject, string body);
    Task<bool> SendVerifyEmail(string toMail, string confirmationLink);
    Task<bool> SendEmailResetAsync(string toEmail, string subject, ApplicationUser user, string currentDate,
        string resetLink,
        string operatingSystem, string browser, string ip, string region, string city, string country);
}