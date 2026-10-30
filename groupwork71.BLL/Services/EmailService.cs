using MimeKit;

namespace groupwork71.BLL.Services;

public class EmailService
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        MimeMessage emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("AttractorDelivery", "ajaxchat676767@mail.ru"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html){Text = message};
        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            await client.ConnectAsync("smtp.mail.ru", 465, true);
            await client.AuthenticateAsync("ajaxchat676767@mail.ru", "IIcfvMGP4IxooOiMCNj1");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}