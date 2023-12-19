using custom_compiler_tutorial.CompilationStage;
using custom_compiler_tutorial.LexerStage;
using custom_compiler_tutorial.ParserStage;
using custom_compiler_tutorial.SyntaxTreeStage;

namespace custom_compiler_tester_tutorial.SyntaxTreeStage
{
    public class EvaluatorTests
    {
        [Theory]
        [InlineData("1", 1)]
        [InlineData("+1", 1)]
        [InlineData("-1", -1)]
        [InlineData("14 + 12", 26)]
        [InlineData("12 - 3", 9)]
        [InlineData("4 * 2", 8)]
        [InlineData("9 / 3", 3)]
        [InlineData("(10)", 10)]
        [InlineData("12 == 3", false)]
        [InlineData("3 == 3", true)]
        [InlineData("12 != 3", true)]
        [InlineData("3 != 3", false)]
        [InlineData("false == false", true)]
        [InlineData("true == false",false)]
        [InlineData("false != false", false)]
        [InlineData("true != false", true)]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("!true", false)]
        [InlineData("!false", true)]
        [InlineData("(a = 10) * a", 100)]
        public void SyntaxFact_GetText_RoundTrips(string text, object expectedValue)
        {
            SyntaxTree syntaxTree = SyntaxTree.Parse(text);
            Compilation compilation = new(syntaxTree);
            Dictionary<VariableSymbol, object> variables = new();
            EvaluationResult result = compilation.Evaluate(variables);
            
            Assert.Empty(result.Diagnostics);
            Assert.Equal(expectedValue, result.Value);
        }
    }
}
