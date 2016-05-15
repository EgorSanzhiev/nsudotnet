using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Rss2Email
{
    class RssItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri Link { get; set; }

        public RssItem(XmlNode itemNode)
        {
            foreach (XmlNode childNode in itemNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "title":
                        Title = childNode.InnerText;
                        break;
                    case "description":
                        Description = childNode.InnerText;
                        break;
                    case "link":
                        Link = new Uri(childNode.InnerText);
                        break;
                }
            }
        }

        public override string ToString()
        {
            return String.Format("{0}\n{1}\n{2}\n", Title, Description, Link);
        }
    }
}
