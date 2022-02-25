using System;

namespace heitech.consoleXt.core.Input.ArgParsing.States
{
    internal class Hyphen : ParserState
    {
        private int _hyphenCount;
        public Hyphen(int initialCount = 1)
            => _hyphenCount = initialCount;

        public override void Accept(char next)
        {
            if (IsAllowedLetterOrDigit(next))
            {
                nextState = new ParameterName(next);
            }
            else if (IsHyphen(next))
            {
                if (_hyphenCount == 1)
                    nextState = new Hyphen(2);
                else
                    IsValid = false; // must not have more than 2 hyphen
            }
            else
                IsValid = false;
        }

        public override IParserState Transform(LineResult result, bool isFinal = false)
            => nextState is null ? this : nextState;
    }
}
