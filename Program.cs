using SaveUtf8Srt.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveUtf8Srt
{
    class Program
    {
        static void Main(string[] args)
        {
            Process();
        }

        private static SrtHandler Handlers { get; } = new SrtHandler();

        private static void Process()
        {
            Console.WriteLine("Enter Path");
            var command = Console.ReadLine();
            if (string.Equals(command, "exit", StringComparison.InvariantCultureIgnoreCase))
                return;

            Console.WriteLine("Start Process");
            Handlers.Execute(command);
            Console.WriteLine("End Process");
            Console.WriteLine("============================================");
            Process();
        }
    }
}
