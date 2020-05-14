using System.Collections.Generic;
using System.IO;
using DummyVirtualMachine;
using Milan;
using Milan.Statements;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace milc
{
    public class StatementVisitor : Visitor<Statement>
    {
        private readonly IReadOnlyDictionary<Identifier, int> _identifiersOffsets;
        private readonly ExpressionVisitor _expressionVisitor;
        private readonly IInstructionStream _instructionStream;

        public StatementVisitor(IReadOnlyDictionary<Identifier, int> identifiersOffsets,
                                ExpressionVisitor expressionVisitor,
                                IInstructionStream instructionStream)
        {
            _identifiersOffsets = identifiersOffsets;
            _expressionVisitor = expressionVisitor;
            _instructionStream = instructionStream;
        }

        private void VisitAssigmentStatement(AssigmentStatement assigment)
        {
            _expressionVisitor.Visit(assigment.Value);
            _instructionStream.Write(IF.St(_identifiersOffsets[assigment.Target]));
        }

        private void VisitIfStatemetnt(IfStatement ifStatement)
        {
            _expressionVisitor.Visit(ifStatement.Condition);
            var ifFalseJumpPosition = _instructionStream.Position;
            _instructionStream.Write(IF.Jmp0(0));

            foreach (var statement in ifStatement.IfTrueStatements)
                Visit(statement);

            var afterIfPosition = _instructionStream.Position;
            _instructionStream.Seek(ifFalseJumpPosition);
            _instructionStream.Write(IF.Jmp0((int)afterIfPosition));
            _instructionStream.Seek(0, SeekOrigin.End);

            if (ifStatement.ElseStatements != null)
            {
                foreach (var elseStatement in ifStatement.ElseStatements)
                    Visit(elseStatement);
            }
        }

        private void VisitWhileStatement(WhileStatement whileStatement)
        {
            var preWhilePosition = _instructionStream.Position;
            _expressionVisitor.Visit(whileStatement.Condition);

            var jmpIfConditionFalsePosition = _instructionStream.Position;
            _instructionStream.Write(IF.Jmp0(0));

            foreach (var statement in whileStatement.Statements)
                Visit(statement);
            _instructionStream.Write(IF.Jmp((int)preWhilePosition));

            var afterWhilePosition = _instructionStream.Position;
            _instructionStream.Seek(jmpIfConditionFalsePosition);
            _instructionStream.Write(IF.Jmp0((int)afterWhilePosition));
            _instructionStream.Seek(0, SeekOrigin.End);
        }

        private void VisitWriteStatement(WriteStatement writeStatement)
        {
            _expressionVisitor.Visit(writeStatement.Value);
            _instructionStream.Write(IF.Prn());
        }
    }
}