namespace Milan.Expressions
{
    public class IdentiferExpression : Expression
    {
        public Identifier Identifier { get; }

        public IdentiferExpression(Identifier identifier)
        {
            Identifier = identifier;
        }
    }
}
