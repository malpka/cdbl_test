using System;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using webapi.Domain;

namespace webapi
{
    public class MailSender
    {
        private readonly AppSettings _appSettings;
        public MailSender(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public void Send(Email email)
        {
            SmtpClient client;
            if(_appSettings.SmtpPort.HasValue)
                client = new SmtpClient(_appSettings.SmtpHost, _appSettings.SmtpPort.Value);
            else 
                client = new SmtpClient(_appSettings.SmtpHost);
            MailAddress from = new MailAddress(email.Sender);
            MailAddress to = new MailAddress(email.Recipients);

            MailMessage message = new MailMessage(from, to);
            message.Body = email.Body;
            message.BodyEncoding =  System.Text.Encoding.UTF8;
            message.Subject = email.Subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            client.Send(message);

            message.Dispose();
        }

    }
}