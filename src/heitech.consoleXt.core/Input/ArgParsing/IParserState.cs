using System;
using System.Text;

namespace heitech.consoleXt.core.Input.ArgParsing
{
    internal interface IParserState
    {
        void Accept(char next);
        bool IsValid { get; }
        ///<summary>
        /// Find the next correct State during the parsing stage 
        ///</summary>
        IParserState Transform(LineResult result, bool isFinal = false);
    }

    internal abstract class ParserState : IParserState
    {
        protected IParserState nextState = null;
        protected readonly StringBuilder _builder = new();

        public bool IsValid { get; protected set; } = true;

        public abstract void Accept(char next);
        public abstract IParserState Transform(LineResult result, bool isFinal = false);

        protected bool IsAllowedLetterOrDigit(char next)
        {
            bool digit = char.IsDigit(next);
            bool letter = char.IsLetter(next);

            return letter || (digit & _builder.Length > 0);
        }

        protected bool IsUnderScore(char next) => next.Equals('_');
        protected bool IsHyphen(char next) => next.Equals('-');
        protected bool IsSpecialCharacter(char next) => char.IsPunctuation(next);

        protected bool IsWhiteSpace(char next) => char.IsWhiteSpace(next);

        protected bool? CanUseUnderscoreOrHyphen(char next)
        {
            if (IsUnderScore(next) || IsHyphen(next))
            {
                if (_builder.Length > 0)
                {
                    _builder.Append(next);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // aint no hyphen / underscore
            return null;
        }
    }
}
