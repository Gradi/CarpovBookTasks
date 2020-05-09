namespace Milan.Parsers.Lexer.Lexemes
{
    public class RightBracketLexeme : Lexeme
    {
        public RightBracketLexeme() : base(")") {}

        protected override int InnerGetHashCode() => 0;
    }
}
