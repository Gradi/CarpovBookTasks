namespace Milan.Parsers.Lexer.Lexemes
{
    public class IdentifierLexeme : Lexeme
    {
        public string Name { get; }

        public IdentifierLexeme(string name) : base("id")
        {
            Name = name;
        }

        public override int GetHashCode() => Name.GetHashCode();

        protected override bool InnerEquals(Lexeme other) => Name == ((IdentifierLexeme)other).Name;
    }
}
