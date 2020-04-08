namespace GrammarLib.Builder
{
    public interface IProductionBuilder
    {
        IProductionBuilder Term(string value);

        IProductionBuilder NonTerm(string value);

        IProductionBuilder To();

        GrammarBuilder Done();
    }
}
