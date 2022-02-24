using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.consoleXt.core;
using heitech.consoleXt.core.Input;

namespace heitech.consoleXt.core.ScriptEnv
{
    public class Loop
    {
        private readonly LoopContext _context;
        private readonly IInputReader _reader;
        private readonly OutputHelperMap _outputMap;
        private readonly IEnumerable<IScript> _scripts;

        internal Loop(IEnumerable<IScript> scripts, OutputHelperMap map, LoopContext context, IInputReader reader)
        {
            _reader = reader;
            _outputMap = map;
            _context = context;
            _scripts = scripts;
        }

        public async Task RunLoop()
        {
            while (_context.IsAlive)
            {
                LineResult nextLine = await _reader.NextLineAsync();
                nextLine.Parse();

                IScript script = _scripts.SingleOrDefault(
                    x => string.Equals(x.Name, nextLine.CommandName, StringComparison.InvariantCultureIgnoreCase)
                );

                if (script == null)
                {
                    await DisplayError(ScriptEnvException.NoScriptFound(nextLine.EnteredLine));
                    continue;
                }

                if (!AreAllParametersAllowed(script, nextLine.Parameters))
                {
                    await DisplayError(ScriptEnvException.ArgumentsDoNotMatch(script, nextLine.Parameters));
                    continue;
                }

                try
                {
                    await script.RunAsync(nextLine.Parameters, _outputMap);
                }
                catch (System.Exception ex)
                {
                    await DisplayError(ScriptEnvException.ScriptError(script, nextLine.Parameters, ex));
                }
            }
        }

        private Task DisplayError(ScriptEnvException exception) 
            => _outputMap[Outputs.Console].WriteAsync(exception);

        private bool AreAllParametersAllowed(IScript script, ParameterCollection parsedParameters)
        {
            var parsed = parsedParameters.ToList();
            var accepted = script.AcceptedParameters.ToList();

            if (parsed.Count > accepted.Count)
                return false;
            
            foreach (var acc in accepted)
            {
                var first = parsed.FirstOrDefault(x => x.Equals(acc));
                if (first != null)
                    parsed.Remove(first);
            }

            // any left in temp copy of parsed means there is arguments that cannot be assigned
            return !parsed.Any(); 
        }
    }
}