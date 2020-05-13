using System;
using DummyVirtualMachine;
using NUnit.Framework;

namespace TestDummyVirtualMachine.Tests
{
    [TestFixture]
    public class TestStack
    {
        [Test]
        public void DefaultIsZeros()
        {
            Stack<int> stack = new Stack<int>(default);

            Assert.That(stack.Count, Is.Zero);
            Assert.That(stack.MaxElements, Is.Zero);
            Assert.That(stack.TryPop(out _), Is.False);
            Assert.That(() => stack.Push(0), Throws.TypeOf<StackOverflowException>());
            Assert.That(() => stack.Pop(), Throws.TypeOf<InvalidOperationException>());
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(6652)]
        public void TestSizes(int count)
        {
            var bytes = new byte[count * sizeof(int)];
            var stack = new Stack<int>(bytes);

            Assert.That(stack.Count, Is.Zero);
            Assert.That(stack.MaxElements, Is.EqualTo(count));
            for (int i = 0; i < count; ++i)
            {
                stack.Push(int.MaxValue);
                Assert.That(stack.Count, Is.EqualTo(i + 1));
            }
            Assert.That(() => stack.Push(0), Throws.TypeOf<StackOverflowException>());

            for(int i = count - 1; i >= 0; --i)
            {
                Assert.That(stack.Pop(), Is.EqualTo(int.MaxValue));
                Assert.That(stack.Count, Is.EqualTo(i));
            }
            Assert.That(stack.Count, Is.Zero);
        }
    }
}
