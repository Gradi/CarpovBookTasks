namespace DummyVirtualMachine.Instructions
{
    public static class InstructionFactory
    {
        public static Instruction Hlt() => HltInstruction.Instance;

        public static Instruction Jmp(int pointer) => new JmpInstruction(pointer);
        public static Instruction Jmp0(int pointer) => new Jmp0Instruction(pointer);
        public static Instruction Jmp1(int pointer) => new Jmp1Instruction(pointer);

        public static Instruction Inn() => InnInstruction.Instance;
        public static Instruction Prn() => PrnInstruction.Instance;

        public static Instruction Ld(int pointer) => new LdInstruction(pointer);
        public static Instruction St(int pointer) => new StInstruction(pointer);

        public static Instruction Add() => AddInstruction.Instance;
        public static Instruction Sub() => SubInstruction.Instance;
        public static Instruction Mul() => MulInstruction.Instance;
        public static Instruction Div() => DivInstruction.Instance;
        public static Instruction Mod() => ModInstruction.Instance;

        public static Instruction Cmp(CmpInstruction.CmpType type) => new CmpInstruction(type);
        public static Instruction CmpEqual() => Cmp(CmpInstruction.CmpType.Equal);
        public static Instruction CmpNotEqual() => Cmp(CmpInstruction.CmpType.NotEqual);
        public static Instruction CmpLessThan() => Cmp(CmpInstruction.CmpType.LessThan);
        public static Instruction CmpLessOrEqualThan() => Cmp(CmpInstruction.CmpType.LessOrEqualThan);
        public static Instruction CmpGreaterThan() => Cmp(CmpInstruction.CmpType.GreaterThan);
        public static Instruction CmpGreaterOrEqualThan => Cmp(CmpInstruction.CmpType.GreaterOrEqualThan);

        public static Instruction Stk(int pointer, int wordCount) => new StkInstruction(pointer, wordCount);

    }
}
