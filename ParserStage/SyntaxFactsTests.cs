using custom_compiler_tutorial.LexerStage;
using custom_compiler_tutorial.ParserStage;
using custom_compiler_tutorial.SyntaxTreeStage;

namespace custom_compiler_tester_tutorial.ParserStage
{
    public class SyntaxFactsTests
    {
        [Theory]
        [MemberData(nameof(GetSyntaxKindData))]
        public void SyntaxFact_GetText_RoundTrips(SyntaxKind kind)
        {
            string? text = SyntaxFacts.GetText(kind);
            if (text == null) return;

            SyntaxToken[] tokens = SyntaxTree.ParseTokens(text).ToArray();
            SyntaxToken token = Assert.Single(tokens);

            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);
        }

        public static IEnumerable<object[]> GetSyntaxKindData()
        {
            SyntaxKind[] kinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));
            foreach (SyntaxKind kind in kinds) yield return new object[] { kind };
        }
    }
}
