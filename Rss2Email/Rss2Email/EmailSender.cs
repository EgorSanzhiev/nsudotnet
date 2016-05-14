using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Rss2Email
{
    class EmailSender
    {
        public static void Send(string to, List<RssItem> updates)
        {
            SmtpClient client = new SmtpClient() 
            {
                Port = 587, 
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = "smtp.mail.ru", 
                EnableSsl = true,
                UseDefaultCredentials = false,
                Timeout = 10000,
                Credentials = new NetworkCredential("rssforwarder", "I'LLBEOVERTHERE")
            };

            StringBuilder builder = new StringBuilder();

            foreach (var ud in updates)
            {
                builder.Append(ud);
            }

            MailMessage message = new MailMessage()
            {
                To = {new MailAddress(to)},
                From = new MailAddress("rssforwarder@mail.ru"),
                Subject = "Rss updates",
                Body = builder.ToString()
            };

            client.Send(message);
        }
    }
}
