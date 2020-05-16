using System;
using System.IO;
using System.IO.Compression;
using CommandLine;
using DummyVirtualMachine;
using DummyVirtualMachine.Instructions.Decoders;
using dvm.Extensions;

namespace dvm
{
    internal static class Program
    {
        public static int Main(string[] args)
        {
            int code = 0;
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(opts => { code = NewMain(opts); });
            return code;
        }

        private static int NewMain(CommandLineOptions options) => GetMachineTicker(options).TickUntilEnd();

        private static MachineTicker GetMachineTicker(CommandLineOptions options)
        {
            if (!File.Exists(options.InputFile))
                throw new FileNotFoundException(null, options.InputFile);

            using Stream fileStream = File.OpenRead(options.InputFile);
            using Stream dataStream = fileStream.IsGzippedStream() ? new GZipStream(fileStream,  CompressionMode.Decompress) : fileStream;
            fileStream.Seek(0, SeekOrigin.Begin);

            var memory = dataStream.ReadAllBytes();
            var machine = new Machine(memory, new ConsoleIOSystem());
            var instructionTable = new InstructionDecoderTable();

            return options.IsDebugEnabled ? new DebuggableMachineTicker(instructionTable, machine) : new MachineTicker(instructionTable, machine);
        }
    }
}
