namespace Milan.Parsers.Syntax
{
    public interface IPatternRecognizerCollection
    {
        IPatternRecognizer<T> GetRecognizer<T>();
    }
}
