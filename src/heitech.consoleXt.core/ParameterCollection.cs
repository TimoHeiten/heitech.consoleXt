using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            => AddParameter(key, "");

        internal void AddParameter(string key, string value)
        {
            string shortName = key.Length == 1 ? key : "";
            string longName = key.Length == 1 ? "" : key;
            Parameter param = new(shortName, longName, false);

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

        static IEqualityComparer<Parameter> simpleEqComparer = new OnlyOneNameMustBeEqComparer();

        public Parameter FindByParameter(Parameter parameter)
            => this.FirstOrDefault(x => simpleEqComparer.Equals(parameter, x));

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

        private class OnlyOneNameMustBeEqComparer : IEqualityComparer<Parameter>
        {
            public bool Equals(Parameter x, Parameter y)
                =>  x.ShortName == y.ShortName || x.LongName == y.LongName;

            public int GetHashCode([DisallowNull] Parameter obj)
                => throw new System.NotSupportedException("this EqualityComparer does not support the HashCode function");
        }
    }
}