using custom_compiler_tutorial.LexerStage;
using custom_compiler_tutorial.ParserStage;
using custom_compiler_tutorial.SyntaxTreeStage;

namespace custom_compiler_tester_tutorial.ParserStage
{
    public class ParserTests
    {
        [Theory]
        [MemberData(nameof(GetBinaryOperatorPairsData))]
        public void Parser_BinaryExpression_HonorsPrecedences(SyntaxKind op1, SyntaxKind op2)
        {
            int op1Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op1);
            int op2Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op2);
            string op1Text = SyntaxFacts.GetText(op1) ?? "";
            string op2Text = SyntaxFacts.GetText(op2) ?? "";
            string text = $"a {op1Text} b {op2Text} c";

            ExpressionSyntax expression = SyntaxTree.Parse(text).Root;

            if (op1Precedence >= op2Precedence)
            {
                using AssertingEnumerator e = new(expression);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "a");
                e.AssertToken(op1, op1Text);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "b");
                e.AssertToken(op2, op2Text);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "c");
            }
            else
            {
                using AssertingEnumerator e = new(expression);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "a");
                e.AssertToken(op1, op1Text);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "b");
                e.AssertToken(op2, op2Text);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "c");
            }
        }

        [Theory]
        [MemberData(nameof(GetUnaryOperatorPairsData))]
        public void Parser_UnaryExpression_HonorsPrecedences(SyntaxKind unaryKind, SyntaxKind binaryKind)
        {
            int unaryPrecedence = SyntaxFacts.GetUnaryOperatorPrecedence(unaryKind);
            int binaryPrecedence = SyntaxFacts.GetBinaryOperatorPrecedence(binaryKind);
            string unaryText = SyntaxFacts.GetText(unaryKind) ?? "";
            string binaryText = SyntaxFacts.GetText(binaryKind) ?? "";
            string text = $"{unaryText} a {binaryText} b";

            ExpressionSyntax expression = SyntaxTree.Parse(text).Root;

            if (unaryPrecedence >= binaryPrecedence)
            {
                using AssertingEnumerator e = new(expression);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.UnaryExpression);
                e.AssertToken(unaryKind, unaryText);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "a");
                e.AssertToken(binaryKind, binaryText);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "b");
            }
            else
            {
                using AssertingEnumerator e = new(expression);
                e.AssertNode(SyntaxKind.UnaryExpression);
                e.AssertToken(unaryKind, unaryText);
                e.AssertNode(SyntaxKind.BinaryExpression);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "a");
                e.AssertToken(binaryKind, binaryText);
                e.AssertNode(SyntaxKind.NameExpression);
                e.AssertToken(SyntaxKind.IdentifierToken, "b");
            }
        }

        public static IEnumerable<object[]> GetUnaryOperatorPairsData()
        {
            foreach (SyntaxKind unary in SyntaxFacts.GetUnaryOperatorKinds())
                foreach (SyntaxKind binary in SyntaxFacts.GetBinaryOperatorKinds())
                    yield return new object[] { unary, binary };
        }

        public static IEnumerable<object[]> GetBinaryOperatorPairsData()
        {
            foreach (SyntaxKind op1 in SyntaxFacts.GetBinaryOperatorKinds())
                foreach (SyntaxKind op2 in SyntaxFacts.GetBinaryOperatorKinds())
                    yield return new object[] { op1, op2 };
        }
    }
}
