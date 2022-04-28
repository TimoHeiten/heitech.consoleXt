using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace heitech.consoleXt.core.Builtins
{
    public class ClearCommand : Script
    {
        public override string Name => "cls";

        public override IEnumerable<Parameter> AcceptedParameters => Array.Empty<Parameter>();

        public override Task RunAsync(ParameterCollection collection, OutputHelperMap output)
        {
            Console.Clear();
            return Task.CompletedTask;
        }
    }
}
