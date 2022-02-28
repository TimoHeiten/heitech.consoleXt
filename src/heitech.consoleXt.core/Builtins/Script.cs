using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace heitech.consoleXt.core.Builtins
{
    public abstract class Script : IScript
    {
        public abstract string Name { get; }
        public abstract IEnumerable<Parameter> AcceptedParameters { get; }

        public abstract Task RunAsync(ParameterCollection collection, OutputHelperMap output);

        public override string ToString()
        {
            var parameters = "[" + string.Join(", ", AcceptedParameters.Select(x => $"[{x.LongName}/{x.ShortName} - {x.Value}]") + "]");
            return $"{Name} - {parameters}";
        }
    }
}
