using System.Collections;
using System.Collections.Generic;
using heitech.consoleXt.core.Helpers;
using heitech.consoleXt.core.Input.ArgParsing;

namespace heitech.consoleXt.core.Input
{
    public class LineResult : IEnumerable<char>
    {
        internal string CommandName { get; set; }
        internal ParameterCollection Parameters { get; set; } = new();
        internal string EnteredLine { get; }

        private readonly Parser _parser;
        internal LineResult(string actualLine)
        {
            EnteredLine = actualLine;
            _parser = new Parser(this);
        }

        internal void Parse() => _parser.Parse();

        public override string ToString()
            => $"[{CommandName} && {Parameters?.Format()}]";

        public IEnumerator<char> GetEnumerator()
            => EnteredLine.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public static implicit operator LineResult(string line) => new LineResult(line);
    }
}