using CommandLine;

namespace milc
{
    public class CommandLineOptions
    {
        [Option('i', HelpText = "Input source file path.", Required = true)]
        public string? InputFile { get; set; }

        [Option('o', HelpText = "Output file path for compiled binary.", Required = false)]
        public string? OutputFile { get; set; }

        [Option("compress", HelpText = "Compresses resulting binary.", Required = false)]
        public bool IsCompress { get; set; }

        [Option("asm", HelpText = "Instead of emitting binary prints resulting program's assembly text.", Required = false)]
        public bool IsAsmDump { get; set; }

        [Option("stack-size", HelpText = "Size of stack (in words).", Required = false, Default = 32)]
        public int StackWordCount { get; set; }
    }
}
