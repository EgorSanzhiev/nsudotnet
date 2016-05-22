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
            DateTime lastUpdate = DateTime.MinValue;
            while (true)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_source);

                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                
                XmlDocument rssDocument = new XmlDocument();

                using (Stream rssStream = response.GetResponseStream())
                {
                    XmlTextReader reader = new XmlTextReader(rssStream);
                    rssDocument.Load(reader);
                }

                XmlNode channelNode = rssDocument.GetElementsByTagName("channel")[0];

                string channelTitle = channelNode.SelectSingleNode("title").InnerText;

                List<RssItem> updates = (from XmlNode childNode in channelNode.ChildNodes 
                                         where childNode.Name == "item" 
                                         select new RssItem(childNode) into item 
                                         where item.PublicationDate > lastUpdate 
                                         select item).ToList();

                lastUpdate = DateTime.Now;

                EmailSender.Send(_subscribers, updates, channelTitle);

                Thread.Sleep(_updatePeriod);

            }
        }
    }
}
