using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using heitech.consoleXt.core.Helpers;

namespace heitech.consoleXt.core.Builtins
{
    public class HelpCommand : IScript
    {
        private readonly IEnumerable<IScript> _scripts;
        public HelpCommand(IEnumerable<IScript> allScripts)
            => _scripts = allScripts;

        public string Name => "help";

        public IEnumerable<Parameter> AcceptedParameters => new Parameter[] { ("r", "reason", false) };

        public Task RunAsync(ParameterCollection collection, OutputHelperMap output)
        {
            var builder = new StringBuilder();
            var reason = collection["r"];
            if (reason != null)
            {
                builder.AppendLine(reason.Value);
            }
            else builder.AppendLine($"No command could be matched - try one of the following!");


            _scripts.ToList().ForEach(x => builder.AppendLine(x.Format()));

            return output[OutputHelperMap.Console].WriteAsync(builder.ToString());
        }
    }
}