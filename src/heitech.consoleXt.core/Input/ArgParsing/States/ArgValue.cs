
using System.Linq;

namespace heitech.consoleXt.core.Input.ArgParsing.States
{
    internal class ArgValue : ParserState
    {
        private readonly string _paramName;
        private QuoteMode _quoteMode;
        public ArgValue(char starter, string parameter, QuoteMode quoteMode = null)
        {
            _quoteMode = quoteMode;
            if (quoteMode == null)
                _builder.Append(starter);

            _paramName = parameter;
        }

        public override void Accept(char next)
        {
            if (QuoteMode.IsQuote(next) && !(_quoteMode is not null && _quoteMode.IsNested(next)))
            {
                if (_quoteMode is null)
                {
                    IsValid = false;
                }
                else
                {
                    bool ended = _quoteMode.Ended(next);
                    if (ended)
                        _quoteMode = null;
                    IsValid = ended;
                };
                return;
            }

            bool? canUse = CanUseUnderscoreOrHyphen(next);
            if (canUse.HasValue)
            {
                if (!canUse.Value)
                    IsValid = false;
                
                return;
            }

            if (IsAllowedLetterOrDigit(next) || next.Equals(','))
            {
                _builder.Append(next);
            }
            else if (IsPassThroughChar(next) && InQuoteMode)
            {
                _builder.Append(next);
            }
            else if (IsWhiteSpace(next))
            {
                if (InQuoteMode) // else whitespaces are allowed and don´t trigger a state change
                    _builder.Append(next);
                else
                    nextState = new Whitespace();
            }
            else
                IsValid = false;
        }

        private bool InQuoteMode => !(_quoteMode is null);

        public override IParserState Transform(LineResult result, bool isFinal = false)
        {
            if (nextState != null || isFinal)
            {
                var param = result.Parameters[_paramName];
                param.Value = _builder.ToString();
                return nextState;
            }

            return this;
        }

        private static char[] allowedOnes = new char[] { '\'', '"', '{', '}', '/', ':', '\\', '?', '!', '.', '$', '€', '|'  };
        private bool IsPassThroughChar(char next)
        {
            return allowedOnes.Any(x => x == next);
        }

    }
    public class QuoteMode
    {
        private char QuoteChar;
        public static QuoteMode Enter(char quoteChar) => new QuoteMode(quoteChar);
        private QuoteMode(char quoteChar)
            => QuoteChar = quoteChar;

        internal static bool IsQuote(char next)
            => next.Equals('\'') || next.Equals('"');

        public bool Ended(char next)
            => next.Equals(QuoteChar);

        public bool IsNested(char next) => next != QuoteChar;
    }
}
