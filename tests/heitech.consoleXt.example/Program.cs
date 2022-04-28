using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.consoleXt.basic_scripts.dotnet_test_scripts;
using heitech.consoleXt.core;
using heitech.consoleXt.core.Builtins;

namespace heitech.consoleXt.example
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("running script loop");
            Skript.Start(prompt: $"timos-shell :>", new DisplayScript(), new DemonstrateIsolation(), new DotnetTestScript());
        }

        private class DisplayScript : Script
        {
            public override string Name => "display";
            public override IEnumerable<Parameter> AcceptedParameters => new Parameter[] { ("c", "content", true)};

            public override async Task RunAsync(ParameterCollection collection, OutputHelperMap output)
            {
                var needle = AcceptedParameters.Single();
                var incoming = collection.Single(x => x.Equals(needle));
                await output[OutputHelperMap.Console].WriteAsync(incoming.Value);
            }
        }

        private class DemonstrateIsolation : Script
        {
            public override string Name => "isl";
            public override IEnumerable<Parameter> AcceptedParameters => Array.Empty<Parameter>();
            public override async Task RunAsync(ParameterCollection collection, OutputHelperMap output)
            {
                await output[OutputHelperMap.Console].WriteAsync("demonstrate isolation --> throw exception on purpose");
                throw new InvalidCastException("cannot cast the parameter here");
            }
        }
    }
}
