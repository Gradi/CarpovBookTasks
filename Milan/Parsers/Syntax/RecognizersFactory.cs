using System;
using System.Collections.Generic;
using Milan.Expressions;
using Milan.Statements;

namespace Milan.Parsers.Syntax
{
    public static class RecognizersFactory
    {
        private static readonly IReadOnlyDictionary<Type, Func<object>> _recognizersFactories;

        static RecognizersFactory()
        {
            _recognizersFactories = new Dictionary<Type, Func<object>>
            {
                { typeof(Program),               () => new ProgramPatternRecognizer() },
                { typeof(StatementCollection),   () => new StatementCollectionPatternRecognizer() },
                { typeof(Statement),             () => new StatementPatternRecognizer() },
                { typeof(ComparisonExpression),  () => new ComparisonExpressionPatternRecognizer() },
                { typeof(Expression),            () => new ExpressionPatternRecognizer() }
            };
        }

        public static IPatternRecognizer<T> Create<T>()
        {
            if (_recognizersFactories.TryGetValue(typeof(T), out var factory))
            {
                return (IPatternRecognizer<T>)factory();
            }
            throw new ArgumentException($"Can't find implementation of \"{typeof(IPatternRecognizer<T>)}\"");
        }
    }
}
