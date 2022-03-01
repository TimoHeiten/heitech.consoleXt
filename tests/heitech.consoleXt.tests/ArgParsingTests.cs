using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using heitech.consoleXt.core;
using heitech.consoleXt.core.Input;
using Xunit;

namespace heitech.consoleXt.tests
{
    public class ArgParsingTests
    {
        [Theory]
        [MemberData(nameof(ParsingTestData))]
        public void Parsing_works_for_different_style_of_commands(LineResult input, LineResult expected)
        {
            // Arrange
            // Act
            input.Parse();

            // Assert
            input.CommandName.Should().Be(expected.CommandName);
            input.Parameters.Should().BeEquivalentTo(expected.Parameters);
            foreach (var parsedParam in input.Parameters)
            {
                var expectedParam = expected.Parameters.SingleOrDefault(x => x.Equals(parsedParam));
                parsedParam.Value.Should().Be(expectedParam.Value);
            }
        }

        [Theory]
        [MemberData(nameof(NonParseableTestData))]
        public void NonParseable_Commands(LineResult input)
        {
            // Arrange
            // Act
            input.Parse();

            // Assert
            input.CommandName.Should().Be("help");
        }

        [Theory]
        [InlineData("s", "s", true)]
        [InlineData("s", "S", true)]
        [InlineData("S", "s", true)]
        [InlineData("long", "long", true)]
        [InlineData("LONG", "long", true)]
        [InlineData("long", "LONG", true)]
        [InlineData("", "", true)]
        [InlineData("s1", "s1", true)]

        [InlineData("s1", "s2", false)]
        [InlineData("CMD1", "cmd2", false)]
        [InlineData("command", "noCommand", false)]
        [InlineData(null, "", false)]
        [InlineData("", null, false)]
        [InlineData(null, null, true)]
        public void Parameter_Eq_Checks(string name, string other, bool expected)
        {
            // Arrange
            Parameter param1 = new(name, "", false);
            Parameter param2 = new(other, "", false);

            // Act
            bool result = param1.Equals(param2);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Parameter_with_Same_Long_and_other_ShortName_are_NE()
        {
            // Arrange
            Parameter param1 = new("name", "", false);
            Parameter param2 = new("", "name", false);

            // Act
            bool equals = param1.Equals(param2);

            // Assert
            equals.Should().BeFalse();
        }

        [Fact]
        public void One_Parameter_is_null_results_in_NE()
        {
            // Arrange
            Parameter param1 = new("name", "", false);
            Parameter param2 = null;

            // Act
            bool equals = param1.Equals(param2);

            // Assert
            equals.Should().BeFalse();
        }

        public static IEnumerable<object[]> ParsingTestData
        {
            get
            {
                var expected = new LineResult("") { CommandName = "abc" };
                expected.Parameters.AddParameter("p", "value");
                yield return new object[]
                {
                    new LineResult("abc -p value"),
                    expected
                };

                expected = new LineResult("") { CommandName = "def" };
                yield return new object[]
                {
                    new LineResult("def"),
                    expected
                };

                expected = new LineResult("") { CommandName = "ghi" };
                expected.Parameters.AddParameter("longname", "value");
                yield return new object[]
                {
                    new LineResult("ghi --longname value"),
                    expected
                };

                expected = new LineResult("") { CommandName = "jkl" };
                expected.Parameters.AddParameter("longname", "value");
                expected.Parameters.AddParameter("l", "short");
                yield return new object[]
                {
                    new LineResult("jkl --longname value -l short"),
                    expected
                };

                expected = new LineResult("") { CommandName = "mno" };
                expected.Parameters.AddParameter("multiple", "a,b,c");
                yield return new object[]
                {
                    new LineResult("mno --multiple a,b,c "),
                    expected
                };

                expected = new LineResult("") { CommandName = "pqr" };
                expected.Parameters.AddParameter("cmd1", "a");
                expected.Parameters.AddParameter("cmd2", "b");
                expected.Parameters.AddParameter("cmd3", "e");
                yield return new object[]
                {
                    new LineResult("pqr --cmd1 a --cmd2 b --cmd3 e"),
                    expected
                };

                expected = new LineResult("") { CommandName = "pqr" };
                expected.Parameters.AddParameter("c", "a,b,c");
                yield return new object[]
                {
                    new LineResult("pqr -c a -c b -c c"),
                    expected
                };

                expected = new LineResult("") { CommandName = "name" };
                expected.Parameters.AddParameter("parameter", "abc_affe-schnee");
                yield return new object[]
                {
                    new LineResult("name --parameter abc_affe-schnee"),
                    expected
                };
                
                expected = new LineResult("") { CommandName = "name" };
                expected.Parameters.AddParameter("parameter", "abc affe schnee");
                yield return new object[]
                {
                    new LineResult("name --parameter 'abc affe schnee'"),
                    expected
                };

                expected = new LineResult("") { CommandName = "with-forward-slash" };
                expected.Parameters.AddParameter("parameter", "abc /affe schnee");
                yield return new object[]
                {
                    new LineResult("with-forward-slash --parameter 'abc /affe schnee'"),
                    expected
                };

                expected = new LineResult("") { CommandName = "curly-braces" };
                expected.Parameters.AddParameter("parameter", "{abc affe schnee}");
                yield return new object[]
                {
                    new LineResult("curly-braces --parameter '{abc affe schnee}'"),
                    expected
                };

                expected = new LineResult("") { CommandName = "json" };
                expected.Parameters.AddParameter("parameter", "{ 'key' : 'value'}");
                yield return new object[]
                {
                    new LineResult("json --parameter \"{ 'key' : 'value'}\""),
                    expected
                };
            }
        }
        public static IEnumerable<object[]> NonParseableTestData
        {
            get
            {
                yield return new object[]
                {
                    new LineResult("2name --parameter abcaffeschnee"),
                };
                yield return new object[]
                {
                    new LineResult("name --parameter 'abc affe '''schnee'")
                };
                yield return new object[]
                {
                    new LineResult("name --parameter 'abc affe '''schnee'")
                };
                yield return new object[]
                {
                    new LineResult("name --parameter _affeschnee")
                };
                // yield return new object[]
                // {
                //     new LineResult("name --parameter -affeschnee")
                // };
                // yield return new object[]
                // {
                //     new LineResult("name --parameter 2affeschnee")
                // };
                // todo
                // -asdad as parameter
                // number start for all
                // hyphen start for all
                // 
            }

        }
    }
}
