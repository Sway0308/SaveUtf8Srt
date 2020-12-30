using SaveUtf8Srt.Handlers;
using System;

namespace SaveUtf8Srt
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
        }

        private static SrtHandler ConvertHandler { get; } = new SrtHandler();
        private static CheckContentHandler CheckHandler { get; } = new CheckContentHandler();

        private static void Start()
        {
            Console.WriteLine("What do you want?");
            Console.WriteLine("1. Convert big5 srt to UTF8 srt");
            Console.WriteLine("2. Check big5 srt and UTF8 srt content");
            var command = Console.ReadLine();
            if (string.Equals(command, "exit", StringComparison.InvariantCultureIgnoreCase))
                return;
            if (string.Equals(command, "1", StringComparison.InvariantCultureIgnoreCase))
                ConvertBig5ToUTF8();
            if (string.Equals(command, "2", StringComparison.InvariantCultureIgnoreCase))
                CheckBig5AndUTF8Content();

            Start();
        }

        private static void ConvertBig5ToUTF8()
        {
            Console.WriteLine("Enter Path");
            var command = Console.ReadLine();
            if (string.Equals(command, "exit", StringComparison.InvariantCultureIgnoreCase))
                return;

            Console.WriteLine("Start Process");
            ConvertHandler.RecusiveSearch(command);
            Console.WriteLine("End Process");
            Console.WriteLine("============================================");
        }

        private static void CheckBig5AndUTF8Content()
        {
            Console.WriteLine("Enter Path");
            var command = Console.ReadLine();
            if (string.Equals(command, "exit", StringComparison.InvariantCultureIgnoreCase))
                return;

            Console.WriteLine("Start Process");
            CheckHandler.RecusiveSearch(command);
            Console.WriteLine("End Process");
            Console.WriteLine("============================================");
        }
    }
}
