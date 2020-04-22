namespace Milan.Parsers.Lexer.Lexemes
{
    public class ConstantLexeme : Lexeme
    {
        public int Value { get; }

        public ConstantLexeme(int value) : base("const")
        {
            Value = value;
        }

        public override int GetHashCode() => Value.GetHashCode();

        protected override bool InnerEquals(Lexeme other) => Value == ((ConstantLexeme)other).Value;
    }
}
