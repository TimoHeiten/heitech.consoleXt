using System.Collections.Generic;
using System.Threading.Tasks;
using heitech.consoleXt.core.Helpers;
using heitech.consoleXt.core.ScriptEnv;

namespace heitech.consoleXt.core.Builtins
{
    internal class KillCommand : IScript
    {
        public string Name => "kill";
        public IEnumerable<Parameter> AcceptedParameters => ParameterCollection.Empty;

        private readonly LoopContext context;
        internal KillCommand(LoopContext context) => this.context = context;
        public async Task RunAsync(ParameterCollection collection, OutputHelperMap output)
        {
            await output[OutputHelperMap.Console].WriteAsync($"EXEC - {this.Format()}");
            context.StopAlive();
        }
    }
}