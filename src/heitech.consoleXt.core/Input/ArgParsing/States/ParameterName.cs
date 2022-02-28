using System;

namespace heitech.consoleXt.core.Input.ArgParsing.States
{
    internal class ParameterName : ParserState
    {
        public ParameterName(char starter)
            => _builder.Append(starter);

        public override void Accept(char next)
        {
            bool? canUse = CanUseUnderscoreOrHyphen(next);
            if (canUse.HasValue)
            {
                if (!canUse.Value)
                    IsValid = false;
                
                return;
            }

            // name can only start with letter or underscore
            if (IsAllowedLetterOrDigit(next) || IsUnderScore(next))
            {
                _builder.Append(next);
            }
            else if (IsWhiteSpace(next))
            {
                var followedParamName = _builder.ToString();
                nextState = new Whitespace(followedParamName);
            }
            else
                IsValid = false;
        }

        public override IParserState Transform(LineResult result, bool isFinal = false)
        {
            if (nextState is null && !isFinal) return this;

            result.Parameters.AddParameter(_builder.ToString());
            return nextState;
        }
    }
}
