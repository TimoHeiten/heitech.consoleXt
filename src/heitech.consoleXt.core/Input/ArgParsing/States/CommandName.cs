namespace heitech.consoleXt.core.Input.ArgParsing.States
{
    internal class CommandName : ParserState
    {
        public override void Accept(char next)
        {
            if (IsHyphen(next))
            {
                if (_builder.Length == 0)
                {
                    IsValid = false;
                    return;
                }
                else
                {
                    _builder.Append(next);
                }
            }
            //else if (char.IsDigit(next) && _builder.Length == 0)
            //{
            //    IsValid = false;
            //}
            else if (IsAllowedLetterOrDigit(next) || IsUnderScore(next) || IsHyphen(next))
            {
                _builder.Append(next);
            }
            else if (IsWhiteSpace(next) && _builder.Length > 0)
            {
                nextState = new Whitespace();
            }
            else
            {
                if (_builder.Length > 0)
                    IsValid = false;
            }
        }

        public override IParserState Transform(LineResult result, bool isFinal = false)
        {
            if (nextState is null && !isFinal) 
                return this;

            result.CommandName = _builder.ToString();
            return nextState;
        }
    }
}
