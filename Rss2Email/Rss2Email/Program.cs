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
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(new Uri("http://wheredidyouslee.livejournal.com/data/rss"));

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            Stream responseStream = response.GetResponseStream();

            using (responseStream)
            {
                List<RssItem> items = RssParser.Parse(responseStream);

                foreach (var item in items)
                {
                    Console.WriteLine(item);
                }

                EmailSender.Send("egorsanzhiev@gmail.com", items);

//                StreamReader reader = new StreamReader(responseStream);
//
//                Console.WriteLine(reader.ReadToEnd());
            }

            Console.WriteLine("DONE");

            Console.ReadLine();
        }
    }
}
