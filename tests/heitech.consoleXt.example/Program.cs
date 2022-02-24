using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.consoleXt.core;

namespace heitech.consoleXt.example
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("running script loop");
            Skript.Start(new DisplayScript(), new DemopnstrateIsolation());
        }

        private class DisplayScript : IScript
        {
            public string Name => "display";

            public IEnumerable<Parameter> AcceptedParameters => new Parameter[] { ("c", "content", true)};

            public async Task RunAsync(ParameterCollection collection, OutputHelperMap output)
            {
                var needle = AcceptedParameters.Single();
                var incoming = collection.Single(x => x.Equals(needle));
                await output[Outputs.Console].WriteAsync(incoming.Value);
            }
        }

        private class DemopnstrateIsolation : IScript
        {
            public string Name => "isl";

            public IEnumerable<Parameter> AcceptedParameters => Array.Empty<Parameter>();

            public async Task RunAsync(ParameterCollection collection, OutputHelperMap output)
            {
                await output[Outputs.Console].WriteAsync("demonstrate isolation --> throw exception on purpose");
                throw new InvalidCastException("cannot cast the parameter here");
            }
        }
    }
}
