using System;
using DummyVirtualMachine;
using DummyVirtualMachine.Instructions.Decoders;

namespace dvm
{
    public class MachineTicker
    {
        protected readonly IInstructionDecoderTable _decoderTable;
        protected readonly Machine _machine;

        protected bool _isInterrupted;

        public MachineTicker(IInstructionDecoderTable decoderTable, Machine machine)
        {
            _decoderTable = decoderTable ?? throw new ArgumentNullException(nameof(decoderTable));
            _machine = machine ?? throw new ArgumentNullException(nameof(machine));

            _isInterrupted = false;
            Console.CancelKeyPress += OnCancelKeyPress;
        }

        public virtual int TickUntilEnd()
        {
            try
            {
                while (IsTickable())
                {
                    _decoderTable.Tick(_machine);
                }
                return 0;
            }
            catch(Exception exception)
            {
                Console.Error.WriteLine("Error on running vm.");
                Console.Error.WriteLine(exception);
                return -1;
            }
        }

        private void OnCancelKeyPress(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            _isInterrupted = true;
            Console.Error.WriteLine("Keyboard interrupt.");
        }

        protected bool IsTickable() => !_isInterrupted && !_machine.IsHalted;
    }
}
