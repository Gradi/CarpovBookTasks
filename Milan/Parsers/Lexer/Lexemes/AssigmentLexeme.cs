namespace Milan.Parsers.Lexer.Lexemes
{
    public class AssigmentLexeme : Lexeme
    {
        public AssigmentLexeme() : base(":=") {}

        protected override int InnerGetHashCode() => 0;
    }
}
