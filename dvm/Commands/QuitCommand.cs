using CommandLine;

namespace dvm.Commands
{
    [Verb("quit", HelpText = "Quits debugging and closes vm.")]
    public class QuitCommand : Command {}
}
