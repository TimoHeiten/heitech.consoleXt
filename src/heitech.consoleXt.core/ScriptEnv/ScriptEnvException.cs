using System;
using heitech.consoleXt.core.Helpers;
using heitech.consoleXt.core.Input;

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

        internal static ScriptEnvException NoScriptFound(LineResult result)
            => new ScriptEnvException($"No Script was registered for {result}");

        internal static ScriptEnvException ScriptError(IScript script, ParameterCollection collection, Exception ex)
            => new ScriptEnvException($"{script.Format()} and {collection.Format()} threw:{Environment.NewLine}{ex.Message}");

        internal static ScriptEnvException ArgumentsDoNotMatch(IScript script, ParameterCollection collection)
            => new ScriptEnvException($"One or more ARGS do not match:{Environment.NewLine}{script.Format()} and {collection.Format()}");
    }
}