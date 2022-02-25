using System;
using System.Text;
using System.Threading.Tasks;

namespace heitech.consoleXt.core.Input
{
    internal class GenericInputReader : IInputReader
    {
        private readonly OutputHelperMap _map;
        internal GenericInputReader(OutputHelperMap map)
            => _map = map;

        public async Task<LineResult> NextLineAsync()
        {
            var builder = new StringBuilder();
            System.Console.WriteLine("-".PadRight(50, '-'));
            await _map[Outputs.Console].WriteAsync($"Next input is required");
            System.Console.WriteLine("-".PadRight(50, '-'));

            return Console.ReadLine();

        //     bool noEnterYet = true;
        //     while (noEnterYet)
        //     {
        //         // todo if key is arrow-up/arrow-down use history
        //         // todo if key is tab try autocomplete on script names
        //         var key = Console.ReadKey();
        //         if (key.Key == ConsoleKey.Enter)
        //             noEnterYet = false;
        //         else
        //             builder.Append(key.KeyChar);
        //     }

        //     return builder.ToString();
        }
    }
}