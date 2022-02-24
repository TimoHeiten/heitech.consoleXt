using System.Collections.Generic;
using System.Threading.Tasks;

namespace heitech.consoleXt.core
{
    public interface IScript
    {
        string Name { get; }
        IEnumerable<Parameter> AcceptedParameters { get; }

        Task RunAsync(ParameterCollection collection, OutputHelperMap output);
    }
}