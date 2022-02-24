using System.Text;

namespace heitech.consoleXt.core.Input.ArgParsing
{
    public class Parser
    {
        private IParserState _current;
        private readonly LineResult _result;
        public Parser(LineResult result)
        {
            _current = new CommandName();
            _result = result;
        }

        public void Parse()
        {
            foreach (var c in _result)
            {
                _current.Accept(c);
                if (!_current.IsValid)
                {
                    _result.CommandName = "help";
                    var parameter = new Parameter("r", "reason", true);
                    parameter.Value = $"Could not parse the command '{_result.EnteredLine}'";
                    _result.Parameters = new ParameterCollection(parameter);
                    break;
                }
                _current = _current.Transform(_result);
            }
            // one final transform for the last value in line
            _ = _current.Transform(_result, isFinal: true);
        }

        private abstract class ParserState : IParserState
        {
            protected readonly StringBuilder _builder = new();
            protected IParserState nextState = null;
            public bool IsValid { get; protected set; } = true;
            public abstract void Accept(char next);
            public abstract IParserState Transform(LineResult result, bool isFinal = false);


            protected bool IsLetter(char next) => char.IsLetterOrDigit(next);
            protected bool IsUnderScore(char next) => next.Equals('_');
            protected bool IsHyphen(char next) => next.Equals('-');
            protected bool IsSpecialCharacter(char next) => char.IsPunctuation(next);

            protected bool IsWhiteSpace(char next) => char.IsWhiteSpace(next);
        }

        private class CommandName : ParserState
        {
            public override void Accept(char next)
            {
                if (IsLetter(next) || IsUnderScore(next) || IsHyphen(next))
                    _builder.Append(char.ToLowerInvariant(next));
                else if (IsWhiteSpace(next))
                    nextState = new Whitespace();
                else
                    IsValid = false;
            }

            public override IParserState Transform(LineResult result, bool isFinal = false)
            {
                if (nextState is null && !isFinal) return this;

                result.CommandName = _builder.ToString();
                return nextState;
            }
        }

        private class Whitespace : ParserState
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
                else if (IsLetter(next) && _followedParamName != null)
                    nextState = new ArgValue(next, _followedParamName);
                else
                    IsValid = false;
            }

            public override IParserState Transform(LineResult result, bool isFinal = false)
                => nextState is null ? this : nextState;
        }

        private class Hyphen : ParserState
        {
            private int _hyphenCount;
            public Hyphen(int initialCount = 1)
                =>_hyphenCount = initialCount;

            public override void Accept(char next)
            {
                if (IsLetter(next))
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

        private class ArgValue : ParserState
        {
            private readonly string _paramName;
            public ArgValue(char starter, string parameter)
            {
                _builder.Append(starter);
                _paramName = parameter;
            }

            public override void Accept(char next)
            {
                if (IsLetter(next) || next.Equals(','))
                {
                    _builder.Append(next);
                }
                else if (IsWhiteSpace(next))
                {
                    nextState = new Whitespace();
                }
                else
                    IsValid = false;
            }

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
        }

        private class ParameterName : ParserState
        {
            public ParameterName(char starter)
                => _builder.Append(starter);

            public override void Accept(char next)
            {
                if (IsLetter(next) || IsUnderScore(next))
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

        internal interface IParserState
        {
            void Accept(char next);
            bool IsValid { get; }
            IParserState Transform(LineResult result, bool isFinal = false);
        }
    }

}