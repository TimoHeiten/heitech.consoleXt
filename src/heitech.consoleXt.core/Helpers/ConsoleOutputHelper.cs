using System;
using System.Threading.Tasks;

namespace heitech.consoleXt.core.Helpers
{
    public class ConsoleOutputHelper : IOutputHelper
    {
        static readonly ConsoleColor starter = Console.ForegroundColor;
        public Task WriteAsync(object obj)
        {
            if (obj is Exception)
                Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"{obj}");
            Console.ForegroundColor = starter;

            return Task.CompletedTask;
        }
    }
}