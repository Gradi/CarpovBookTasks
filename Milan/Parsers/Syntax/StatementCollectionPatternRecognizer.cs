using System.Collections.Generic;
using Milan.Parsers.Lexer;
using Milan.Parsers.Lexer.Lexemes;
using Milan.Statements;

namespace Milan.Parsers.Syntax
{
    public class StatementCollectionPatternRecognizer : BasePatternRecognizer<StatementCollection>
    {
        public override Result<StatementCollection> Recognize(ILexemeEnumerator lexemes, ProgramBuilder programBuilder, IPatternRecognizerCollection recognizers)
        {
            var statements = new List<Statement>();
            var statementRecognizer = recognizers.GetRecognizer<Statement>();

            while(true)
            {
                var statement = statementRecognizer.Recognize(lexemes, programBuilder, recognizers);
                if (!statement.HasValue)
                    return Fail(statement);

                if (statement.Value == null)
                {
                    if (statements.Count != 0)
                        return new StatementCollection(statements);
                    return Fail("Can't parse statements list.");
                }

                statements.Add(statement.Value);

                if (lexemes.MoveNext())
                {
                    if (!(lexemes.Current is SemicolonLexeme))
                    {
                        lexemes.Return();
                        return new StatementCollection(statements);
                    }
                }
                else
                {
                    return new StatementCollection(statements);
                }
            }
        }
    }
}
