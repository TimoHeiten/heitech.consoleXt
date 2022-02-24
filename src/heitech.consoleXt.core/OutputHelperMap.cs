using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace heitech.consoleXt.core
{
    public static class Outputs
    {
        public const string Console  = "CONSOLE";
    }
    public class OutputHelperMap
    {
        private static readonly object _concurrencyToken = new();
        private static readonly Dictionary<string, IOutputHelper> _map;
        static OutputHelperMap()
        {
            _map = new();
        }

        public OutputHelperMap(IEnumerable<OutputRegistrar> all)
            => all.ToList().ForEach(x => { if (!_map.ContainsKey(x.Key)) _map.Add(x.Key, x.Helper); });

        public IOutputHelper this[string key]
        {
            get => _map[key];
        }
    }

    public class OutputRegistrar
    {
        public OutputRegistrar(string key, IOutputHelper helper)
        {
            Key = key;
            Helper = helper;
        }
        public string Key { get; }
        public IOutputHelper Helper { get; }
    }

    public interface IOutputHelper
    {
        // todo define interface - maybe with bridge or visitor
        Task WriteAsync(object obj);
    }
}