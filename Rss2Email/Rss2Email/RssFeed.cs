using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Xml;

namespace Rss2Email
{
    class RssFeed
    {
        private Uri _source;

        private string _channelTitle;

        private string _channelDescription;

        private TimeSpan _updatePeriod;

        private List<MailAddress> _subscribers;

        public RssFeed(Uri rssSource, TimeSpan updatePeriod)
        {
            _source = rssSource;

            _updatePeriod = updatePeriod;

            _subscribers = new List<MailAddress>();
        }

        public void Subscribe(params string[] emails)
        {
            foreach (var address in emails)
            {
                _subscribers.Add(new MailAddress(address));       
            }
        }

        public void StartChecking()
        {
            while (true)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri("http://wheredidyouslee.livejournal.com/data/rss"));

                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                
                XmlDocument rssDocument = new XmlDocument();

                using (Stream rssStream = response.GetResponseStream())
                {
                    XmlTextReader reader = new XmlTextReader(rssStream);
                    rssDocument.Load(reader);
                }

                XmlNode channelNode = rssDocument.GetElementsByTagName("channel")[0];

                _channelTitle = channelNode.SelectSingleNode("title").InnerText;

                _channelDescription = channelNode.SelectSingleNode("description").InnerText;

                List<RssItem> updates = (from XmlNode childNode in channelNode.ChildNodes 
                                         where (childNode.Name == "item") 
                                         select new RssItem(childNode)).ToList();

                EmailSender.Send(_subscribers, updates);

                Thread.Sleep(_updatePeriod);

            }
        }
    }
}
