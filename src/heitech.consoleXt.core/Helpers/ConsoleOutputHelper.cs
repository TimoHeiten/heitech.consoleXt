using System;
using System.Threading.Tasks;

namespace heitech.consoleXt.core.Helpers
{
    public class ConsoleOutputHelper : IOutputHelper
    {
        static ConsoleColor starter = Console.ForegroundColor;
        public Task WriteAsync(object obj)
        {
            // todo see baisclogger from zer0mqxt, change the console color with regards to the obj type
            if (obj is Exception)
                Console.ForegroundColor = ConsoleColor.Red;

            System.Console.WriteLine(obj);

            Console.ForegroundColor = starter;
            return Task.CompletedTask;
        }
    }
}