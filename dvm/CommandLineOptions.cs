using CommandLine;

namespace dvm
{
    public class CommandLineOptions
    {
        [Option('i', HelpText = "Path to a compiled binary.", Required = true)]
        public string? InputFile { get; set; }

        [Option("debug", HelpText = "Enables debugging interface.")]
        public bool IsDebugEnabled { get; set; }
    }
}
