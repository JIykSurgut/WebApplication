    using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;

namespace Models
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            MailMessage msg = new MailMessage(
                from:   "sbis86@mail.ru",
                to:      message.Destination,
                subject: message.Subject,
                body:    message.Body
                ) { IsBodyHtml = true };

            new SmtpClient(host: "smtp.mail.ru", port: 25)
            {
                Credentials = new NetworkCredential(
                    userName: "sbis86@mail.ru",
                    password: "*******" //bbbbb
                    ),
                EnableSsl = true
            }.Send(msg);

            return Task.FromResult(0);
        }
    }
}
