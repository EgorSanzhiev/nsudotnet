using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonSerializer
{
    [Serializable]
    class MyClass
    {
        public int a { get; set; }
        [NonSerialized]
        public string b;
        private int g;
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyClass m = new MyClass
            {
                a = 10,
                b = "hello",
            };


            m.Serialize("my.json");

            DateTime dt = DateTime.Now;

            dt.Serialize("dt.json");
        }
    }
}
