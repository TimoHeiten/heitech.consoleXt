using System;
using System.IO;
using System.Threading.Tasks;
using heitech.consoleXt.core.ScriptEnv;

namespace heitech.consoleXt.core.Helpers
{
    public class FileOutputHelper : IOutputHelper
    {
        static IOutputHelper _console = new ConsoleOutputHelper();
        public async Task WriteAsync(object obj)
        {
            if (obj is FileParameter contents)
            {
                try
                {
                    await File.AppendAllTextAsync(contents.Path, contents.Contents);
                }
                catch (System.Exception ex)
                {
                    await _console.WriteAsync(ScriptEnvException.From(ex));
                }
            }
            else
            {
                await _console.WriteAsync(ScriptEnvException.IncorrectFileOutputParameter(obj));
                await _console.WriteAsync(obj);
            }
        }

        public record FileParameter(string Path, string Contents);
    }
}
