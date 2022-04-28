using System.Collections.Generic;
using System.Linq;

namespace heitech.consoleXt.core
{
    public class OutputHelperMap
    {
        private static readonly Dictionary<string, IOutputHelper> _map = new();
        public OutputHelperMap(IEnumerable<OutputDescriptor> all)
            => all.ToList().ForEach(x => { if (!_map.ContainsKey(x.Key)) _map.Add(x.Key, x.Helper); });

        public IOutputHelper this[string key]
        {
            get => _map[key];
        }

        public const string Console = "CONSOLE";
        public const string File = "FILE";
    }
}