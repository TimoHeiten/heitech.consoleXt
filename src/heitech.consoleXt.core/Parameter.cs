
using System;

namespace heitech.consoleXt.core
{
    ///<summary>
    /// Defines a parameter for a script
    ///</summary>
    public class Parameter : IEquatable<Parameter>
    {
        public string LongName { get; }
        public string ShortName { get; }
        internal bool IsMandatory { get; set; }
        internal string Value { get; set; }
        public Parameter(string sh, string lg, bool isMandatory)
        {
            IsMandatory = isMandatory;
            ShortName = sh;
            LongName = lg;
        }

        public bool TryParse<T>(Func<string, (T val, bool sccss)> parser, out T result)
        {
            result = default;
            var tpl = parser(Value);
            if (tpl.sccss)
            {
                result = tpl.val;
                return true;
            }

            return false;
        }

        public bool Equals(Parameter other)
        {
            return other is null 
                  ? false
                  : other.ShortName.Equals(ShortName, StringComparison.InvariantCultureIgnoreCase)
                    && other.LongName.Equals(LongName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode() => LongName.GetHashCode() | ShortName.GetHashCode();

        public static implicit operator Parameter((string _short, string _long, bool mandatory) definition)
             => new Parameter(definition._short, definition._long, definition.mandatory);
    }
}