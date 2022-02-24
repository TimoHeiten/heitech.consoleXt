using System.Collections.Generic;
using System.Linq;
using heitech.consoleXt.core.Builtins;
using heitech.consoleXt.core.Helpers;
using heitech.consoleXt.core.Input;
using heitech.consoleXt.core.ScriptEnv;

namespace heitech.consoleXt.core
{
    public static class Skript
    {
        public static async void Start(params OutputRegistrar[] outputs)
        {
            try
            {
                var allOuts = outputs.Concat(new[] { new OutputRegistrar(Outputs.Console, new ConsoleOutputHelper()) });
                OutputHelperMap outputMap = new(allOuts);
                var loopContext = new LoopContext();

                // gather IScripts
                var allScripts = GatherAllScripts();
                var help = new HelpCommand(allScripts);
                var kill = new KillCommand(loopContext);
                allScripts = allScripts.Concat(new IScript[] { help, kill });

                var reader = new GenericInputReader(outputMap);

                // start the loop
                var loop = new Loop(allScripts, outputMap, loopContext, reader);
                await loop.RunLoop();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
                throw;
            }
            // prepare desired outputs and loopContext
        }

        private static IEnumerable<IScript> GatherAllScripts()
        {
            // todo typescan...use scrutor?
            return Enumerable.Empty<IScript>();
        }
    }
}