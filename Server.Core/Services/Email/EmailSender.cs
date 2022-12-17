using Microsoft.Extensions.Options;
using Server.Entities.Options;
using System.Net;
using System.Net.Mail;

namespace Server.Core.Services.Email;

public sealed class EmailSender : IEmailSender
{
    private readonly EmailOptions _emailOptions;

    public EmailSender(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }

    public async Task<bool> SendEmailAsyncWithCheck(string to, string subject, string message)
    {
        try
        {
            CheckOptions();
            return await SendAsync(to, subject, message);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            CheckOptions();
            _ = await SendAsync(email, subject, htmlMessage).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
        }
    }

    private void CheckOptions()
    {
        if (string.IsNullOrEmpty(_emailOptions.EmailFrom))
        {
            throw new ArgumentNullException("Email From is Null");
        }        
        if (string.IsNullOrEmpty(_emailOptions.EmailFromName))
        {
            throw new ArgumentNullException("Email From Name is Null");
        }        
        if (string.IsNullOrEmpty(_emailOptions.Port))
        {
            throw new ArgumentNullException("Email Port is Null");
        }        
        if (string.IsNullOrEmpty(_emailOptions.Password))
        {
            throw new ArgumentNullException("Email Password is Null");
        }
    }

    private async Task<bool> SendAsync(string email, string subject, string message)
    {
        var msg = new MailMessage
        {
            From = new MailAddress(_emailOptions.EmailFrom, _emailOptions.EmailFromName),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };

        msg.To.Add(new MailAddress(email));

        using var smtp = new SmtpClient(_emailOptions?.Host, int.Parse(_emailOptions!.Port))
        {
            UseDefaultCredentials = false,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(_emailOptions!.EmailFrom, _emailOptions!.Password)
        };
        await smtp.SendMailAsync(msg);

        return true;
    }
}
