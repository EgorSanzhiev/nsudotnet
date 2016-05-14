using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Rss2Email
{
    class RssParser
    {
        public static List<RssItem> Parse(Stream rssStream)
        {
            XmlDocument rssDocument = new XmlDocument();

            using (XmlTextReader reader = new XmlTextReader(rssStream))
            {
                rssDocument.Load(reader);
            }

            XmlNode channelNode = rssDocument.GetElementsByTagName("channel")[0];

            return (from XmlNode childNode in channelNode.ChildNodes 
                    where (childNode.Name == "item") 
                    select new RssItem(childNode)).ToList();
        }
    }
}
