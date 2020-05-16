using CommandLine;

namespace dvm.Commands
{
    [Verb("tick", HelpText = "Ticks specifiied number of times.")]
    public class TickCommand : Command
    {
        [Option('n', HelpText = "Number of ticks to tick", Default = 1)]
        public int TickCount { get; set; }
    }
}
