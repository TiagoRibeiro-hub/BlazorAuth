namespace Server.Core.Services.Email;
public interface IEmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
{
    Task<bool> SendEmailAsyncWithCheck(string to, string subject, string message);
}
