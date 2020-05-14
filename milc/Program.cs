using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using CommandLine;
using DummyVirtualMachine;

namespace milc
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(NewMain);
        }

        private static void NewMain(CommandLineOptions options)
        {
            using var sourceReader = new StreamReader(File.OpenRead(options.InputFile), Encoding.UTF8);
            using var instructionStream = GetInstructionStream(options);

            new ProgramWriter(sourceReader, instructionStream, options.StackWordCount).Write();
        }

        private static IInstructionStream GetInstructionStream(CommandLineOptions options)
        {
            if (options.IsAsmDump)
            {
                return new TextInstructionStream(Console.Out);
            }

            var outputFile = options.OutputFile ?? Path.GetFileNameWithoutExtension(options.InputFile) + ".bin";
            Stream outputStream;
            var fileStream = File.Create(outputFile);

            if (options.IsCompress)
            {
                var memoryStream = new MemoryStreamWithEvent();
                memoryStream.OnClose += (sender, memStream) =>
                {
                    using var gzipStream = new GZipStream(fileStream, CompressionLevel.Optimal);
                    memStream.CopyTo(gzipStream);
                };
                outputStream = memoryStream;
            }
            else
            {
                outputStream = fileStream;
            }

            return new InstructionStream(outputStream);
        }
    }
}
