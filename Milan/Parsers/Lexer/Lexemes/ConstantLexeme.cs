namespace Milan.Parsers.Lexer.Lexemes
{
    public class ConstantLexeme : Lexeme
    {
        public int Value { get; }

        public ConstantLexeme(int value) : base("const")
        {
            Value = value;
        }

        protected override bool InnerEquals(Lexeme other) => Value == ((ConstantLexeme)other).Value;

        protected override int InnerGetHashCode() => Value.GetHashCode();
    }
}
