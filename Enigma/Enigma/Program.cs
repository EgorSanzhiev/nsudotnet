using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enigma
{
    class Program
    {
        public static readonly string Usage = string.Format("Usage\n{0}\n{1}",
            "Enigma.exe encrypt <algorithm> <input filename> <ouput filename>",
            "Enigma.exe decrypt <algorithm> <input filename> <key filename> <output filename>");

        static void Main(string[] args)
        {
            Crypter crypter = new Crypter(args[1]);

            try
            {
                switch (args[0])
                {
                    case "encrypt":
                        crypter.Encrypt(args[2], args[3]);
                        break;
                    case "decrypt":
                        crypter.Decrypt(args[2], args[3], args[4]);
                        break;
                    default:
                        throw new ArgumentException(Usage);
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("DONE");

            Console.ReadLine();
        }
    }
}
