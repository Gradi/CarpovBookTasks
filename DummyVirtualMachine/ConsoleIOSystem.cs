using System;

namespace DummyVirtualMachine
{
    public class ConsoleIOSystem :  IOSystem
    {
        public void Write(int value) => Console.WriteLine(value);

        public int Read()
        {
            while (true)
            {
                Console.Write("-> ");
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    return result;
                }
                Console.WriteLine("Invalid input. Try again.");
            }
        }
    }
}
