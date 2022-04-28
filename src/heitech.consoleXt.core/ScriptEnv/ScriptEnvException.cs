using System;
using heitech.consoleXt.core.Helpers;
using heitech.consoleXt.core.Input;
using static heitech.consoleXt.core.Helpers.FileOutputHelper;

namespace heitech.consoleXt.core.ScriptEnv
{
    public class ScriptEnvException : Exception
    {
        private ScriptEnvException(string message)
            : base(message)
        { }

        private ScriptEnvException(string message, Exception inner)
            : base(message, inner)
        { }

        internal static ScriptEnvException From(Exception ex) 
            => new ScriptEnvException("Exception was thrown. See inner for details", ex);

        internal static ScriptEnvException NoScriptFound(LineResult result)
            => new ScriptEnvException($"No Script was registered for {result}");

        internal static ScriptEnvException ScriptError(IScript script, ParameterCollection collection, Exception ex)
            => new ScriptEnvException($"{script.Format()} and {collection.Format()} threw:{Environment.NewLine}{ex}");

        internal static ScriptEnvException ArgumentsDoNotMatch(IScript script, ParameterCollection collection)
            => new ScriptEnvException($"One or more ARGS do not match:{Environment.NewLine}{script.Format()} and {collection.Format()}");

        internal static ScriptEnvException IncorrectFileOutputParameter(object obj)
            => new ScriptEnvException($"FileOutputHelper expects an object of '{nameof(FileParameter)}' but got {obj.GetType().Name}");
    }
}