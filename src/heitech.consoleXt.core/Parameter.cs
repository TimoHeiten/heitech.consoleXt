
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
        internal bool IsMandatory { get; }

        private string _value;
        internal string Value
        {
            get => _value;
            set
            {
                if (string.IsNullOrWhiteSpace(_value))
                {
                    _value = value;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(value))
                        _value += $",{value}";
                }
            }
        }
        public Parameter(string sh, string lg, bool isMandatory = false)
        {
            LongName = lg;
            ShortName = sh;
            IsMandatory = isMandatory;
        }

        ///<summary>
        /// Tries to parse the internal value with respect to the supplied parse callback.
        ///</summary>
        public static bool TryParse<T>(Parameter me, Func<string, (T val, bool sccss)> parser, out T result)
        {
            result = default;

            if (me is null || parser is null)
                return false;

            result = default;
            var (val, sccss) = parser(me.Value);
            if (sccss)
            {
                result = val;
                return true;
            }

            return false;
        }

        public bool Equals(Parameter other)
        {
            if (other is null) return false;

            bool shortNameExists = !string.IsNullOrWhiteSpace(ShortName);
            bool longNameExists = !string.IsNullOrWhiteSpace(LongName);

            bool lnEq() => string.Equals(LongName, other.LongName, StringComparison.OrdinalIgnoreCase);
            bool snEq() => string.Equals(ShortName, other.ShortName, StringComparison.OrdinalIgnoreCase);

            if (shortNameExists && longNameExists)
                return lnEq() && snEq();
            else if (longNameExists)
                return lnEq();
            else
                return snEq();
             
        }

        public override int GetHashCode() 
            => LongName.GetHashCode() | ShortName.GetHashCode();

        public static implicit operator Parameter((string _short, string _long, bool mandatory) definition)
             => new Parameter(definition._short, definition._long, definition.mandatory);
    }
}