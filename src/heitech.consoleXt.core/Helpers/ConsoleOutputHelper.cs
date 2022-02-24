using System.Threading.Tasks;

namespace heitech.consoleXt.core.Helpers
{
    public class ConsoleOutputHelper : IOutputHelper
    {
        public Task WriteAsync(object obj)
        {
            // todo see baisclogger from zer0mqxt, change the console color with regards to the obj type
            System.Console.WriteLine(obj);
            return Task.CompletedTask;
        }
    }
}