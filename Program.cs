using SaveUtf8Srt.Handlers;
using System;
using Gatchan.Base.Standard.Base;

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
        private static MoveBig5SrtHandler MoveBig5SrtHandler { get; } = new MoveBig5SrtHandler();

        private static void Start()
        {
            Console.WriteLine("What do you want?");
            Console.WriteLine("1. Convert big5 srt to UTF8 srt");
            Console.WriteLine("2. Check big5 srt and UTF8 srt content");
            Console.WriteLine("3. Move big5 srt");
            var command = Console.ReadLine();
            if (string.Equals(command, "exit", StringComparison.InvariantCultureIgnoreCase))
                return;
            if (string.Equals(command, "1", StringComparison.InvariantCultureIgnoreCase))
                ConvertBig5ToUTF8();
            if (string.Equals(command, "2", StringComparison.InvariantCultureIgnoreCase))
                CheckBig5AndUTF8Content();
            if (command.SameText("3"))
                MoveBig5Srt();

            Console.WriteLine();
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

        private static void MoveBig5Srt()
        {
            Console.WriteLine("Enter Srt Path");
            var srtPath = Console.ReadLine();
            Console.WriteLine("Enter Backup Path");
            var backupPath = Console.ReadLine();
            if ("exit".SameTextOr(srtPath, backupPath))
                return;

            Console.WriteLine("Start Process");
            MoveBig5SrtHandler.MoveSrt(srtPath, backupPath);
            Console.WriteLine("End Process");
            Console.WriteLine("============================================");
        }
    }
}
