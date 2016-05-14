using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NumberGuesser
{
    class NumberGuesser
    {
        private class StatisticsItem
        {
            public int Answer { get; set; }
            public string LessOrGreater { get; set; }
        }

        private static readonly string[] Offenses =
        {
            "Grow a pair, {0}!",
            "{0}, you are a worthless loser!",
            "{0}, this is pathetic.",
            "I'm losing my patience, {0}!",
            "SAY WRONG NUMBER AGAIN! I DARE YOU, I DOUBLE DARE YOU, {0}, SAY WRONG NUMBER ONE MORE GODDAMN TIME!"
        };

        private int _number;

        private StatisticsItem[] _statistics = new StatisticsItem[1000];

        public void Run()
        {
            Console.Write("Welcome to the NumberGuesser! Please enter your name: ");

            string username = Console.ReadLine();

            Random random = new Random();

            _number = random.Next(101);

            int trialCount = 0;

            DateTime startTime = DateTime.Now;

            while (trialCount < _statistics.Length - 1)
            {
                Console.Write("Make your guess: ");
                string userInput = Console.ReadLine();

                if (userInput == "q")
                {
                    Console.WriteLine("Sorry for being rude.");
                    return;
                }

                int userAnswer;

                if (!int.TryParse(userInput, out userAnswer))
                {
                    Console.Write("Please enter a valid number! ");

                    continue;
                }

                ++trialCount;

                if (userAnswer != _number)
                {
                    string lessOrGreater = (userAnswer < _number) ? "less" : "greater";

                    Console.WriteLine(String.Format("Guess again! Your number is {0}", lessOrGreater));

                    if (trialCount % 4 == 0)
                    {

                        int offenseIndex = random.Next(Offenses.Length);

                        Console.WriteLine(String.Format(Offenses[offenseIndex], username));
                    }

                    StatisticsItem statsItem = new StatisticsItem();

                    statsItem.Answer = userAnswer;

                    statsItem.LessOrGreater = lessOrGreater;

                    _statistics[trialCount - 1] = statsItem;
                }
                else
                {
                    TimeSpan spentTime = DateTime.Now - startTime;

                    Console.WriteLine(String.Format("Finally! You guessed right, {0}. It took you {1:0.00} minutes to guess the number", username, spentTime.TotalMinutes));
                    Console.WriteLine("Trials: {0}", trialCount);

                    PrintStatistics();

                    return;
                }
            }
        }

        private void PrintStatistics()
        {
            int index = 0;

            StatisticsItem item = _statistics[index];

            Console.WriteLine("Statistics:");

            while (item != null)
            {
                ++index;

                Console.WriteLine(String.Format("{0} is {1} than {2}", item.Answer, item.LessOrGreater, _number));

                item = _statistics[index];
            }
        }
    }
}