using System.Collections;
using System.Collections.Generic;
using heitech.consoleXt.core.Helpers;
using heitech.consoleXt.core.Input.ArgParsing;
using heitech.consoleXt.core.Input.ArgParsing.States;

namespace heitech.consoleXt.core.Input
{
    ///<summary>
    /// represents the current Line that will be used as input
    ///</summary>
    public class LineResult : IEnumerable<char>
    {
        internal string CommandName { get; set; }
        internal ParameterCollection Parameters { get; set; } = new();
        internal string EnteredLine { get; }

        internal LineResult(string actualLine)
            => EnteredLine = actualLine;

        ///<summary>
        /// Try to make sense of the contents of the line and interpret as command
        ///</summary>
        internal void Parse()
        {
            IParserState _current = new CommandName();
            List<char> parsedCorrectly = new();

            foreach (var character in this)
            {
                if (character == '\0')
                    continue;

                _current.Accept(character);
                parsedCorrectly.Add(character);
                if (!_current.IsValid)
                {
                    this.CommandName = "help";
                    var parameter = new Parameter("r", "reason", true);
                    parameter.Value = $"Could not parse the command '{EnteredLine}' failed at {new string(parsedCorrectly.ToArray())}";
                    this.Parameters = new ParameterCollection(parameter);
                    return;
                }
                _current = _current.Transform(this);
            }

            // one final transform for the last value in line
            _ = _current.Transform(this, isFinal: true);
        }

        public override string ToString()
            => $"[{CommandName} && {Parameters?.Format()}]";

        public IEnumerator<char> GetEnumerator()
            => EnteredLine.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public static implicit operator LineResult(string line) 
            => new LineResult(line);
    }
}