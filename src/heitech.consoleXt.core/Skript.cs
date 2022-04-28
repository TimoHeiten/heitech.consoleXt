using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech.consoleXt.core.Builtins;
using heitech.consoleXt.core.Helpers;
using heitech.consoleXt.core.Input.Autocomplete;
using heitech.consoleXt.core.ScriptEnv;

namespace heitech.consoleXt.core
{
    public static class Skript
    {
        ///<summary>
        /// Run only with default settings and infer scripts from Interface implementations
        ///</summary>
        public static async void Start(string prompt)
            => await start(Enumerable.Empty<IScript>(), Array.Empty<OutputDescriptor>(), prompt);

        ///<summary>
        /// Add Scripts and defined outputHelpers
        ///</summary>
        public static async void Start(IEnumerable<IScript> starterScripts, IEnumerable<OutputDescriptor> outputs, string prompt)
            => await start(starterScripts, outputs.ToArray(), prompt);

        ///<summary>
        /// Add Scripts yourself instead of scanning for their types
        ///</summary>
        public static async void Start(string prompt, params IScript[] starterScripts)
            => await start(starterScripts, Array.Empty<OutputDescriptor>(), prompt);
        public static async void Start(params IScript[] starterScripts)
            => await start(starterScripts, Array.Empty<OutputDescriptor>());

        ///<summary>
        /// Add only defined outputHelpers
        ///</summary>
        public static async void Start(params OutputDescriptor[] outputs)
            => await start(Enumerable.Empty<IScript>(), outputs);
        public static async void Start(string prompt, params OutputDescriptor[] outputs)
            => await start(Enumerable.Empty<IScript>(), outputs, prompt);

        private static async Task start(IEnumerable<IScript> starterScripts, OutputDescriptor[] outputs, string prompt = "$>")
        {
            try
            {
                OutputHelperMap outputMap = CreateOutputHelpers(outputs);
                var loopContext = new LoopContext();

                // gather IScripts
                var allScripts = GatherAllScripts(ignore: starterScripts);
                var help = new HelpCommand(allScripts);
                var kill = new KillCommand(loopContext);
                var clear = new ClearCommand();
                allScripts = allScripts.Concat(new IScript[] { help, kill, clear });

                var reader = new ReadInChars(allScripts, prompt);

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

        private static OutputHelperMap CreateOutputHelpers(OutputDescriptor[] outputs)
        {
            var allOuts = outputs.Concat
               (
                   new[]
                   {
                        new OutputDescriptor(OutputHelperMap.Console, new ConsoleOutputHelper()),
                        new OutputDescriptor(OutputHelperMap.File, new FileOutputHelper())
                   }
               );
            return new(allOuts);
        }

        private static IEnumerable<IScript> GatherAllScripts(IEnumerable<IScript> ignore)
        {
            // todo typescan...use scrutor?
            return ignore.Concat(Enumerable.Empty<IScript>());
        }
    }
}