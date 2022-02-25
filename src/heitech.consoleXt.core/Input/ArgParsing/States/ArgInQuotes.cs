// using System;
// using System.Collections.Generic;

// namespace heitech.consoleXt.core.Input.ArgParsing.States
// {
//     internal class ArgInQuotes : ParserState
//     {
//         private char _quoteChar;
//         private QuoteState _current;
//         private IParserState _next = null;
//         public ArgInQuotes(char quoteChar)
//         {
//             _current = quoteChar.Equals('\'') ? new SingleQuote() : new DoubleQuote();
//         }

//         public override void Accept(char next)
//         {
//             _current.Accept(next);
//             this.IsValid = _current.IsValid;
//         }

//         public override IParserState Transform(LineResult result, bool isFinal = false)
//         {
//             _next = _current.Transform(result, isFinal);
//             return _next != null ? _next : this;
//         }

//         private abstract class QuoteState : IParserState
//         {
//             protected List<char> _value = new();
//             protected IParserState me = null;
//             public bool IsValid { get; protected set; } = true;
//             public abstract void Accept(char next);

//             public abstract IParserState Transform(LineResult result, bool isFinal = false);
//             internal string Result() => new string(_value.ToArray());
//         }

//         private class SingleQuote : QuoteState
//         {
//             public override void Accept(char next)
//             {
//                 if ()
//             }
//         }

//         private class DoubleQuote : QuoteState
//         {
//             public override void Accept(char next)
//             {
                
//             }
//         }


//     }
// }
