using CommandLine;

namespace dvm.Commands
{
    [Verb("fill-stack", HelpText = "Fill stack with specified byte (Doesn't change stack pointer value).")]
    public class FillStackCommand : Command
    {
        [Option('v', HelpText = "Value to write", Default = (byte)0xFF)]
        public byte Value { get; set; }
    }
}
