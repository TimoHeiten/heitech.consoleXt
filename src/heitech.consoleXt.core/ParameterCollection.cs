using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace heitech.consoleXt.core
{
    public class ParameterCollection : IEnumerable<Parameter>
    {
        private readonly List<Parameter> parameters;

        internal ParameterCollection(IEnumerable<Parameter> parameters)
            => this.parameters = parameters.ToList();

        internal ParameterCollection(params Parameter[] parameters)
            => this.parameters = parameters.ToList();

        internal void AddParameter(string key)
        {
            AddParameter(key, "");
        }

        internal void AddParameter(string key, string value)
        {
            string sn = key.Length == 1 ? key : "";
            string ln = key.Length == 1 ? "" : key;
            Parameter param = new(sn, ln, false);
            var existing = parameters.FirstOrDefault(x => x.Equals(param));
            if (existing != null)
            {
                if (!string.IsNullOrWhiteSpace(value))
                    existing.Value += $",{value}";
            }
            else
            {
                param.Value = value;
                parameters.Add(param);
            }
        }

        public Parameter this[string key]
        {
            get
            {
                var first = this.FirstOrDefault(x => x.LongName == key || x.ShortName == key);
                return first;
            }
        }

        public IEnumerator<Parameter> GetEnumerator()
            => parameters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public static ParameterCollection Empty => new (Enumerable.Empty<Parameter>());
    }
}