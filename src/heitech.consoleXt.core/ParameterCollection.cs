using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace heitech.consoleXt.core
{
    public class ParameterCollection : IEnumerable<Parameter>
    {
        private readonly List<Parameter> parameters;

        internal ParameterCollection(IEnumerable<Parameter> parameters)
        {
            this.parameters = parameters.ToList();
        }

        internal ParameterCollection(params Parameter[] parameters)
        {
            this.parameters = parameters.ToList();
        }

        internal void AddParameter(string key)
        {
            string sn = key.Length == 1 ? key : "";
            string ln = key.Length == 1 ? "" : key;
            parameters.Add(new(sn, ln, false));
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
        {
            return parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static ParameterCollection Empty => new (Enumerable.Empty<Parameter>());
    }
}