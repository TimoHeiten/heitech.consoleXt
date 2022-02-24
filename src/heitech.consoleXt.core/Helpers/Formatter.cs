using System;
using System.Collections.Generic;
using System.Linq;

namespace heitech.consoleXt.core.Helpers
{
    public static class Formatter
    {
        public static string Format(this IScript script)
        {
            return $"Script [{script.Name} -{script.AcceptedParameters.Format()}]";
        }

        public static string Format(this IEnumerable<Parameter> cltn)
        {
            return string.Join(" - ", cltn.Select(x => $"'{x.ShortName}->{x.Value}'"));
        }

    }
}