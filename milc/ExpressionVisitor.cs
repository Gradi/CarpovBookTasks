using System;
using System.Collections.Generic;
using DummyVirtualMachine;
using DummyVirtualMachine.Instructions;
using Milan;
using Milan.Expressions;
using Milan.Expressions.Enums;
using IF = DummyVirtualMachine.Instructions.InstructionFactory;

namespace milc
{
    public class ExpressionVisitor : Visitor<Expression>
    {
        private readonly IReadOnlyDictionary<Constant, int> _constantsOffsets;
        private readonly IReadOnlyDictionary<Identifier, int> _identifiersOffsets;
        private readonly IInstructionStream _instructionStream;

        public ExpressionVisitor(IReadOnlyDictionary<Constant, int> constantsOffsets,
                                 IReadOnlyDictionary<Identifier, int> identifiersOffsets,
                                 IInstructionStream instructionStream)
        {
            _constantsOffsets = constantsOffsets;
            _identifiersOffsets = identifiersOffsets;
            _instructionStream = instructionStream;
        }

        private void VisitComparisonExpression(ComparisonExpression comparison)
        {
            Visit(comparison.LeftOperand);
            Visit(comparison.RightOperand);
            _instructionStream.Write(IF.Cmp(comparison.Type switch
            {
                ComparisonType.Equal => CmpInstruction.CmpType.Equal,
                ComparisonType.NotEqual => CmpInstruction.CmpType.NotEqual,
                ComparisonType.Less => CmpInstruction.CmpType.LessThan,
                ComparisonType.LessEqual => CmpInstruction.CmpType.LessOrEqualThan,
                ComparisonType.Greater => CmpInstruction.CmpType.GreaterThan,
                ComparisonType.GreaterEqual => CmpInstruction.CmpType.GreaterOrEqualThan,
                var val => throw new ArgumentOutOfRangeException(nameof(comparison), $"Unsupported comparison type ({val}).")
            }));
        }

        private void VisitConstantExpression(ConstantExpression constant)
        {
            _instructionStream.Write(IF.Ld(_constantsOffsets[constant.Constant]));
        }

        private void VisitIdentifierExpression(IdentifierExpression identifier)
        {
            _instructionStream.Write(IF.Ld(_identifiersOffsets[identifier.Identifier]));
        }

        private void VisitMathExpression(MathExpression math)
        {
            Visit(math.LeftOperand);
            Visit(math.RightOperand);

            Instruction inst;
            switch (math.Type)
            {
                case OperationType.Plus:
                    inst = IF.Add();
                    break;
                case OperationType.Minus:
                    inst = IF.Sub();
                    break;
                case OperationType.Multiply:
                    inst = IF.Mul();
                    break;
                case OperationType.Divide:
                    inst = IF.Div();
                    break;
                case OperationType.Modulo:
                    inst = IF.Mod();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(math), $"Unsupported math operation value ({math.Type}).");
            }
            _instructionStream.Write(inst);
        }

        private void VisitReadExpression(ReadExpression read)
        {
            _instructionStream.Write(IF.Inn());
        }
    }
}
