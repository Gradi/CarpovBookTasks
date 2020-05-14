namespace Milan.Expressions
{
    public class IdentifierExpression : Expression
    {
        public Identifier Identifier { get; }

        public IdentifierExpression(Identifier identifier)
        {
            Identifier = identifier;
        }
    }
}
