namespace Milan.Parsers.Lexer.Lexemes
{
    public class SemicolonLexeme : Lexeme
    {
        public SemicolonLexeme() : base(";") {}

        protected override int InnerGetHashCode() => 0;
    }
}
