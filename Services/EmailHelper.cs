using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Threading.Tasks;
using BigJobbs.Interfaces;

namespace BigJobbs.Services
{
    public class EmailHelper : IMail
    {
        public EmailHelper()
        {

        }
        public async Task SendMail(string recieverEmail, string messageSubject, string messageBody)
        {
            try
            {
                // Plug in your email service here to send an email.
                var emailMessage = new MimeMessage();

                //
                emailMessage.From.Add(new MailboxAddress("BigJobbz", "kentekz61@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("Application Status", recieverEmail));
                emailMessage.Subject = messageSubject;
                emailMessage.Body = new TextPart("plain") { Text = messageBody };

                await Task.Run(() =>
                {

                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate("kentekz61@gmail.com", "19962929");
                        client.Send(emailMessage);
                        client.Disconnect(true);

                    }
                });
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}