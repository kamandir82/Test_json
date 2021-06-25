using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Timers;

namespace Test_json
{
    class Program
    {
        private static Timer aTimer;
        public static String s1 = "---", s2 = "---", s3 = "---", s4 = "---", s5 = "---";
        static void Main(string[] args)
        {
            // Create a timer and set a two second interval.
            Console.Title = "RIG STATUS";
            Console.SetWindowSize(43, 22);
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 10000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;

            Console.WriteLine("Press the Enter key to exit the program at any time... ");
            Console.ReadLine();


        }
        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {

            Console.Clear();
            Console.WriteLine("--- RIG STATUS ---");
            Console.WriteLine("time: {0}", e.SignalTime);
            using (WebClient webClient = new System.Net.WebClient())
            {
                WebClient n = new WebClient();
                var json = n.DownloadString("https://api.nanopool.org/v1/eth/user/0x72231B6ac234051A2FE05D4c6af0b248D8f659a4");
                string valueOriginal = Convert.ToString(json);
                //Console.WriteLine(json);
                dynamic jdata = JObject.Parse(json);
                Console.WriteLine();
                //Console.WriteLine(string.Concat("Balance: " + jdata.data.balance, " ETH"));
                Console.Write("Status: ");
                if (jdata.status == "true")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("OK");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR");
                    for (int i = 0; i < 5; i++) Console.Beep();
                }
                Console.ResetColor();
                Console.Write("Balance: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(string.Concat(jdata.data.balance, " ETH"));
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("Avg hashrate: ");
                Console.WriteLine(string.Concat(" 1: ", jdata.data.avgHashrate.h1));
                Console.WriteLine(string.Concat(" 3: ", jdata.data.avgHashrate.h3));
                Console.WriteLine(string.Concat(" 6: ", jdata.data.avgHashrate.h6));
                Console.WriteLine(string.Concat("12: ", jdata.data.avgHashrate.h12));
                Console.WriteLine(string.Concat("24: ", jdata.data.avgHashrate.h24));
            
                Console.WriteLine();

                Console.WriteLine();
                WebClient n2 = new WebClient();
                var json2 = n2.DownloadString("https://min-api.cryptocompare.com/data/price?fsym=ETH&tsyms=BTC,USD,EUR");
                string valueOriginal2 = Convert.ToString(json2);
                dynamic jdata2 = JObject.Parse(json2);
                s5 = s4;
                s4 = s3;
                s3 = s2;
                s2 = s1;
                s1 = jdata2.USD;
                Console.WriteLine("ETH/USD: ");
                Console.WriteLine(string.Concat(" 1: ", s1));
                Console.WriteLine(string.Concat(" 2: ", s2));
                Console.WriteLine(string.Concat(" 3: ", s3));
                Console.WriteLine(string.Concat(" 4: ", s4));
                Console.WriteLine(string.Concat(" 5: ", s5));

                System.Console.ReadKey();
                //Console.WriteLine(string.Concat("Balance: ", jdata.status, " " + jdata.data.balance, " " + jdata.data.avgHashrate.h1));
            }
        }
    }
}
