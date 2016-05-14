using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumberGuesser
{
    class Program
    {
        static void Main(string[] args)
        {
            NumberGuesser game = new NumberGuesser();

            game.Run();

            Console.ReadLine();
        }
    }
}
