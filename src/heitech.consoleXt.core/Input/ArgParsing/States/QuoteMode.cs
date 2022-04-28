using System;

namespace heitech.consoleXt.core.Input.ArgParsing.States
{
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
