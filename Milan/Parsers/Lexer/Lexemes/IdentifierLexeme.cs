namespace Milan.Parsers.Lexer.Lexemes
{
    public class IdentifierLexeme : Lexeme
    {
        public string Name { get; }

        public IdentifierLexeme(string name) : base("id")
        {
            Name = name;
        }

        protected override bool InnerEquals(Lexeme other) => Name == ((IdentifierLexeme)other).Name;

        protected override int InnerGetHashCode() => Name?.GetHashCode() ?? 0;
    }
}
