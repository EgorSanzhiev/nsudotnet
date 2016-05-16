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
        private static readonly MailAddress Sender = new MailAddress("rssforwarder@mail.ru");

        public static void Send(List<MailAddress> receivers,List<RssItem> updates, string channelTitle)
        {
            if (updates.Count == 0)
                return;

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
                From = Sender,
                Subject = String.Format("Updates from {0}", channelTitle),
                Body = builder.ToString()
            };

            foreach (var address in receivers)
            {
                message.To.Add(address);
            }

            client.Send(message);
        }
    }
}
