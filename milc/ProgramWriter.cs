using System.Collections.Generic;
using System.IO;
using DummyVirtualMachine;
using Milan;
using Milan.Parsers.Syntax;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace milc
{
    public class ProgramWriter
    {
        private readonly TextReader _inputSource;
        private readonly IInstructionStream _instructionStream;

        private readonly int _stackWordCount;

        private readonly Dictionary<Constant, int> _constantOffsets;
        private readonly Dictionary<Identifier, int> _identifierOffsets;
        private readonly StatementVisitor _statementVisitor;

        public ProgramWriter(TextReader inputSource, IInstructionStream instructionStream, int stackWordCount)
        {
            _inputSource = inputSource;
            _instructionStream = instructionStream;
            _stackWordCount = stackWordCount;

            _constantOffsets = new Dictionary<Constant, int>();
            _identifierOffsets = new Dictionary<Identifier, int>();
            _statementVisitor = new StatementVisitor(_identifierOffsets,
                                                     new ExpressionVisitor(_constantOffsets, _identifierOffsets, instructionStream),
                                                     instructionStream);
        }

        public void Write()
        {
            var program = SyntaxParser.Parse(_inputSource);
            _instructionStream.Write(IF.Stk(0, 0));
            _instructionStream.Write(IF.Jmp(0));

            foreach (var constant in program.Constants)
            {
                _constantOffsets[constant] = (int)_instructionStream.Position;
                _instructionStream.Write(constant.Value);
            }

            foreach (var identifier in program.Identifiers)
            {
                _identifierOffsets[identifier] = (int)_instructionStream.Position;
                _instructionStream.Write((int)0);
            }

            var entryPointPosition = _instructionStream.Position;
            foreach (var statement in program.Statements)
            {
                _statementVisitor.Visit(statement);
            }
            _instructionStream.Write(IF.Hlt());

            var stackPosition = _instructionStream.Position;
            for(int i = 0; i < _stackWordCount; ++i)
                _instructionStream.Write((int)0);

            _instructionStream.Seek(0);
            _instructionStream.Write(IF.Stk((int)stackPosition, _stackWordCount));
            _instructionStream.Write(IF.Jmp((int)entryPointPosition));
        }
    }
}
