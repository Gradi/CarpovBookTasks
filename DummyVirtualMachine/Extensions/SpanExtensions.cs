using System;

namespace DummyVirtualMachine.Extensions
{
    internal static class SpanExtensions
    {
        public static int AsInt32(this Span<byte> span) => BitConverter.ToInt32(span);
        public static int AsInt32(this ReadOnlySpan<byte> span) => BitConverter.ToInt32(span);
        public static int AsInt32(this Memory<byte> span) => BitConverter.ToInt32(span.Span);
        public static int AsInt32(this ReadOnlyMemory<byte> span) => BitConverter.ToInt32(span.Span);
    }
}
