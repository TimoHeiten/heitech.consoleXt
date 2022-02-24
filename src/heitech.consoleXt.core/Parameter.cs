
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
        public Parameter(string sh, string lg, bool isMandatory)
        {
            LongName = lg;
            ShortName = sh;
            IsMandatory = isMandatory;
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
            if (other is null) return false;

            bool shortNameExists = !string.IsNullOrWhiteSpace(ShortName);
            bool longNameExists = !string.IsNullOrWhiteSpace(LongName);

            Func<bool> lnEq = () => string.Equals(LongName, other.LongName, StringComparison.OrdinalIgnoreCase);
            Func<bool> snEq = () => string.Equals(ShortName, other.ShortName, StringComparison.OrdinalIgnoreCase);

            if (shortNameExists && longNameExists)
                return lnEq() && snEq();
            else if (longNameExists)
                return lnEq();
            else
                return snEq();
             
        }

        public override int GetHashCode() => LongName.GetHashCode() | ShortName.GetHashCode();

        public static implicit operator Parameter((string _short, string _long, bool mandatory) definition)
             => new Parameter(definition._short, definition._long, definition.mandatory);
    }
}