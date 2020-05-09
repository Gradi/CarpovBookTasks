namespace Milan.Parsers.Lexer.Lexemes
{
    public class LeftBracketLexeme : Lexeme
    {
        public LeftBracketLexeme() : base("(") {}

        protected override int InnerGetHashCode() => 0;
    }
}
