using System;
using System.Diagnostics;
using System.Linq;
using CommandLine;
using DummyVirtualMachine;
using DummyVirtualMachine.Instructions;
using DummyVirtualMachine.Instructions.Decoders;
using dvm.Commands;
using dvm.Extensions;

namespace dvm
{
    public class DebuggableMachineTicker : MachineTicker
    {
        private readonly Stopwatch _stopwatch;

        private DateTimeOffset _startDate;
        private bool _isDebugRunning;

        public DebuggableMachineTicker(IInstructionDecoderTable decoderTable, Machine machine) : base(decoderTable, machine)
        {
            _stopwatch = new Stopwatch();
            _isDebugRunning = true;
        }

        public override int TickUntilEnd()
        {
            _stopwatch.Reset();
            _startDate = DateTimeOffset.Now;
            _isInterrupted = true;

            while (_isDebugRunning)
            {
                if (!IsTickable())
                    ReadEvalCommand();
                else
                {
                    while (IsTickable())
                        TickOneTime();
                }
            }

            return -1;
        }

        private void ReadEvalCommand()
        {
            Console.Error.WriteLine("Welcome to Debugger interface 3000! Type 'help' to get more information.");
            while (!IsTickable() && _isDebugRunning)
            {
                Console.Write("-> ");
                var commandArgs = Console.ReadLine().AsArgs();

                Parser.Default.ParseArguments(commandArgs, CommandsCollection.Types.ToArray())
                    .MapResult((ContinueCommand command) => HandleContinueCommand(command),
                               ((DumpCommand command) => HandleDumpCommand(command)),
                               (QuitCommand command) => HandleQuitCommand(command),
                               (TickCommand command) => HandleTickCommand(command),
                               (FillStackCommand command) => HandleFillStackCommand(command),
                               errs => 1);
            }
        }

        /// <returns>true if ticked without errors.</returns>
        private bool TickOneTime()
        {
            bool isGood = true;
            _stopwatch.Start();
            try
            {
                _decoderTable.Tick(_machine);
            }
            catch(Exception exception)
            {
                _isInterrupted = true;
                Console.Error.WriteLine("Error on running vm.");
                Console.Error.WriteLine(exception);
                isGood = false;
            }
            _stopwatch.Stop();
            return isGood;
        }

        private int HandleContinueCommand(ContinueCommand command)
        {
            _isInterrupted = false;
            return 0;
        }

        private int HandleDumpCommand(DumpCommand dump)
        {
            Console.Error.WriteLine($"Start time..................: {_startDate:f}");
            Console.Error.WriteLine($"Now.........................: {DateTimeOffset.Now:f}");
            Console.Error.WriteLine($"Run time....................: {_stopwatch.Elapsed}");
            Console.Error.WriteLine($"Ticks.......................: {_machine.TickCount}");
            Console.Error.WriteLine($"Memory size.................: {_machine.Memory.Length}");
            Console.Error.WriteLine($"IP..........................: 0x{_machine.InstructionPointer:X8}");
            Console.Error.WriteLine($"Halted......................: {_machine.IsHalted}");
            Console.Error.WriteLine($"Stack size..................: {_machine.Stack?.MaxElements}");
            Console.Error.WriteLine($"Stack elements count........: {_machine.Stack?.Count}");
            Console.Error.WriteLine($"Stack elements available....: {_machine.Stack?.Available}");
            Console.Error.WriteLine($"Stack dump..................:");
            if (_machine.Stack != null)
            {
                foreach (var value in _machine.Stack.DumpValues())
                    Console.Error.WriteLine(value.ToString("X8"));
            }

            Console.Error.WriteLine($"Memory dump.................:");
            const int maxWidth = 16;
            using var hexDump = new HexDumpStream(Console.Error, false);
            foreach (var instruction in _decoderTable.DecodeAll(_machine.Memory))
            {
                string? GetComment() => hexDump.Position == _machine.InstructionPointer ? "<- IP" : null;

                if (instruction.Instruction != null)
                {
                    hexDump.FlushLine();
                    hexDump.Write(instruction.Instruction, GetComment());
                    hexDump.FlushLine();
                }
                else
                {
                    hexDump.Write(_machine.Memory[instruction.Offset], GetComment());
                    if (hexDump.LineSizeInBytes >= maxWidth)
                        hexDump.FlushLine();
                }
            }

            return -1;
        }

        private int HandleQuitCommand(QuitCommand quit)
        {
            _isDebugRunning = false;
            return 0;
        }

        private int HandleTickCommand(TickCommand tick)
        {
            if (tick.TickCount > 0)
            {
                bool isGood = true;
                for(int i = 0; i < tick.TickCount && isGood; ++i)
                    isGood = TickOneTime();
            }
            return 0;
        }

        private int HandleFillStackCommand(FillStackCommand command)
        {
            if (_machine.Stack == null)
                Console.Error.WriteLine("Stack is not initialized.");
            else
            {
                for(int i = 0; i < _machine.Stack.Memory.Length; ++i)
                    _machine.Stack.Memory.Span[i] = command.Value;
            }
            return 0;
        }
    }
}
