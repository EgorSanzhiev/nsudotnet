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
        static void Main(string[] args)
        {

            RssFeed feed = new RssFeed(new Uri("http://wheredidyouslee.livejournal.com/data/rss"), TimeSpan.FromMinutes(5));

            feed.Subscribe("sanzhiev@ccfit.nsu.ru", "egorsanzhiev@mail.ru");

            feed.StartChecking();
        }
    }
}
