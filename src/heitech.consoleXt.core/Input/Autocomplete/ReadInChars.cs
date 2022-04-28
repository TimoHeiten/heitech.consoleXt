using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace heitech.consoleXt.core.Input.Autocomplete
{
    public class ReadInChars : IInputReader
    {
        private readonly string _prompt;
        public ReadInChars(IEnumerable<IScript> scripts, string prompt)
        {
            _prompt = prompt;
            ReadLine.AutoCompletionHandler = new AutoComplete(scripts);
        }

        public async Task<LineResult> NextLineAsync()
        {
            await Task.CompletedTask;
            string input = ReadLine.Read(_prompt);
            ReadLine.AddHistory(input);

            return new LineResult(input);
        }

        private class AutoComplete : IAutoCompleteHandler
        {
            private readonly IEnumerable<IScript> _scripts;
            public AutoComplete(IEnumerable<IScript> scripts)
                => _scripts = scripts;

            public char[] Separators { get; set; } = new char[] { '-', ' ', '.', '/' };

            public string[] GetSuggestions(string text, int index)
            {
                var cmdStartsWith = _scripts.FirstOrDefault(x => x.Name.StartsWith(text));
                var cmd = _scripts.FirstOrDefault(x => text.StartsWith(x.Name));

                if (cmd is not null)
                    return ToFlatArray(cmd, text, index);
                else if (cmdStartsWith is not null)
                    return new[] { $"{cmdStartsWith.Name}" };
                else 
                    return _scripts.Select(x => x.Name).ToArray();
            }

            private string[] ToFlatArray(IScript script, string text, int index)
            {
                var list = new List<string>(capacity: script.AcceptedParameters.Count() * 2);

                foreach (var item in script.AcceptedParameters)
                {
                    list.Add($"-{item.ShortName}");
                    list.Add($"--{item.LongName}");
                }

                return list.ToArray();
            }
        }
    }
}
