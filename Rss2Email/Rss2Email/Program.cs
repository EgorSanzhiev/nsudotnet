using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Rss2Email
{
    class Program
    {
        //Arguments - RSS source, update period in minutes, list of e-mails
        static void Main(string[] args)
        {
            int updatePeriod = int.Parse(args[1]);
            RssFeed feed = new RssFeed(new Uri(args[0]), TimeSpan.FromMinutes(updatePeriod));

            string[] subscribers = new string[args.Length - 2];

            Array.Copy(args, 2, subscribers, 0, subscribers.Length);

            feed.Subscribe(subscribers);

            feed.StartChecking();
        }
    }
}
