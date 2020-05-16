using System;
using System.Collections.Generic;
using System.Linq;

namespace dvm.Commands
{
    public static class CommandsCollection
    {
        public static readonly IReadOnlyCollection<Type> Types;

        static CommandsCollection()
        {
            var command = typeof(Command);
            var types = command.Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && t != command && t.IsClass && command.IsAssignableFrom(t))
                .ToArray();

            Types = Array.AsReadOnly(types);
        }
    }
}
