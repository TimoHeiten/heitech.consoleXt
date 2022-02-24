using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.consoleXt.core.Builtins;
using heitech.consoleXt.core.Helpers;
using heitech.consoleXt.core.Input;
using heitech.consoleXt.core.ScriptEnv;

namespace heitech.consoleXt.core
{
    public static class Skript
    {
        ///<summary>
        /// Run only with default settings and infer scripts from Interface implementations
        ///</summary>
        public static async void Start()
            => await start(Enumerable.Empty<IScript>(), Array.Empty<OutputRegistrar>());

        ///<summary>
        /// Add Scripts and defined outputHelpers
        ///</summary>
        public static async void Start(IEnumerable<IScript> starterScripts, params OutputRegistrar[] outputs)
            => await start(starterScripts, outputs);

        ///<summary>
        /// Add Scripts yourself instead of scanning for their types
        ///</summary>
        public static async void Start(params IScript[] starterScripts)
            => await start(starterScripts, Array.Empty<OutputRegistrar>());

        ///<summary>
        /// Add only defined outputHelpers
        ///</summary>
        public static async void Start(params OutputRegistrar[] outputs)
            => await start(Enumerable.Empty<IScript>(), outputs);

        private static async Task start(IEnumerable<IScript> starterScripts, OutputRegistrar[] outputs)
        {
            try
            {
                var allOuts = outputs.Concat(new[] { new OutputRegistrar(Outputs.Console, new ConsoleOutputHelper()) });
                OutputHelperMap outputMap = new(allOuts);
                var loopContext = new LoopContext();

                // gather IScripts
                var allScripts = GatherAllScripts(ignore: starterScripts);
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

        private static IEnumerable<IScript> GatherAllScripts(IEnumerable<IScript> ignore)
        {
            // todo typescan...use scrutor?
            return ignore.Concat(Enumerable.Empty<IScript>());
        }
    }
}