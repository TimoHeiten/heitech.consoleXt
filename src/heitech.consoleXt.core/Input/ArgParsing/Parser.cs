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
        }
    }

    internal class CommandName : IParserState
    {
        private readonly StringBuilder _builder = new();
        private IParserState nextState = null;
        public bool IsValid { get; private set; } = true;
        public void Accept(char next)
        {
            if (char.IsLetterOrDigit(next) || next.Equals('_'))
                _builder.Append(char.ToLowerInvariant(next));
            else if (char.IsWhiteSpace(next))
                nextState = new Whitespace();
            else
                IsValid = false;
        }

        public IParserState Transform(LineResult result)
        {
            if (nextState is null) return this;

            result.CommandName = _builder.ToString();
            return nextState;
        }
    }

    internal class Whitespace : IParserState
    {
        IParserState nextState = null;
        private string _followedParamName;
        public Whitespace(string followedParamName = null)
            => _followedParamName = followedParamName;

        public void Accept(char next)
        {
            if (char.IsWhiteSpace(next))
                nextState = new Whitespace(); // multiple whitespaces is allowed
            else if (next.Equals('-'))
                nextState = new Hyphen();
            else if (char.IsLetter(next) && _followedParamName != null)
                nextState = new ArgValue(next, _followedParamName);
            else
                IsValid = false;
        }

        public bool IsValid { get; private set; } = true;

        public IParserState Transform(LineResult result)
            => nextState is null ? this : nextState;
    }

    internal class Hyphen : IParserState
    {
        private int _hyphenCount;
        IParserState nextState = null;
        public Hyphen(int initialCount = 1)
        {
            _hyphenCount = initialCount;
        }
        public bool IsValid { get; private set; } = true;

        public void Accept(char next)
        {
            if (char.IsLetterOrDigit(next))
            {
                nextState = new ParameterName(next);
            }
            else if (next.Equals('-'))
            {
                if (_hyphenCount == 1)
                    nextState = new Hyphen(2);
                else
                    IsValid = false; // must not have more than 2 hyphen
            }
            else
                IsValid = false;
        }

        public IParserState Transform(LineResult result)
            => nextState is null ? this : nextState;
    }

    internal class ArgValue : IParserState
    {
        private readonly StringBuilder _builder;
        private IParserState nextState = null;
        private readonly string _paramName;
        public ArgValue(char starter, string parameter)
        {
            _builder.Append(starter);
            _paramName = parameter;
        }

        public bool IsValid { get; private set; }

        public void Accept(char next)
        {
            if (char.IsLetterOrDigit(next))
            {
                _builder.Append(next);
            }
            else if (char.IsWhiteSpace(next))
            {
                nextState = new Whitespace();
            }
            else
                IsValid = false;
        }

        public IParserState Transform(LineResult result)
        {
            if (nextState != null)
            {
                var param = result.Parameters[_paramName];
                param.Value = _builder.ToString();
                return nextState;
            }

            return this;
        }
    }

    internal class ParameterName : IParserState
    {
        private readonly StringBuilder _builder = new();
        private IParserState nextState = null;
        public bool IsValid { get; private set; }
        public ParameterName(char starter)
            => _builder.Append(starter);

        public void Accept(char next)
        {
            if (char.IsLetterOrDigit(next) || next.Equals('_'))
            {
                _builder.Append(next);
            }
            else if (char.IsWhiteSpace(next))
            {
                var followedParamName = _builder.ToString();
                nextState = new Whitespace(followedParamName);
            }
            else
                IsValid = false;
        }

        public IParserState Transform(LineResult result)
        {
            if (nextState is null) return this;

            result.Parameters.AddParameter(_builder.ToString());
            return nextState;
        }
    }

    internal interface IParserState
    {
        void Accept(char next);
        bool IsValid { get; }
        IParserState Transform(LineResult result);
    }
}