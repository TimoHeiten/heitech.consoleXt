using System.Collections.Generic;
using System.Linq;

namespace heitech.consoleXt.core.Helpers
{
    public static class Formatter
    {
        public static string Format(this IScript script)
        {
            string formatted = script.Name;
            string parameters = script.AcceptedParameters.Any() ? $" - {script.AcceptedParameters.Format()}" : "";
            return $"[{formatted}{parameters}]";
        }

        public static string Format(this IEnumerable<Parameter> cltn)
        {
            string select(Parameter p)
            {
                if (!string.IsNullOrWhiteSpace(p.Value))
                    return $"'{p.ShortName} | {p.LongName} -> {p.Value}'";
                else
                    return $"'{p.ShortName} | {p.LongName}'";
            }
            return string.Join(" - ", cltn.Select(select));;
        }
    }
}