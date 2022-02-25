using System;

namespace heitech.consoleXt.core.Input.ArgParsing.States
{

    internal class Whitespace : ParserState
    {
        private string _followedParamName;
        public Whitespace(string followedParamName = null)
            => _followedParamName = followedParamName;

        public override void Accept(char next)
        {
            if (IsWhiteSpace(next))
                nextState = new Whitespace(); // multiple whitespaces is allowed
            else if (IsHyphen(next))
                nextState = new Hyphen();
            else if (_followedParamName != null)
            {
                if (IsAllowedLetterOrDigit(next))
                {
                    nextState = new ArgValue(next, _followedParamName);
                }
                else if (QuoteMode.IsQuote(next))
                    nextState = new ArgValue(next, _followedParamName, QuoteMode.Enter(next));
                else
                    IsValid = false;
            }
            else
                IsValid = false;
        }

        public override IParserState Transform(LineResult result, bool isFinal = false)
            => nextState is null ? this : nextState;
    }
}
