using System;
using Tinos.Api.Common.Interfaces;
using Tinos.Api.Shell.Shell;

namespace Tinos.Api.Shell.BuiltIn
{
    /// <summary>
    /// A command for displaying help information.
    /// </summary>
    public class HelpCmd : ICommand
    {
        private readonly CommandRegistry _commandRegistry;

        public HelpCmd(CommandRegistry commandRegistry)
        {
            _commandRegistry = commandRegistry
                ?? throw new ArgumentNullException(nameof(commandRegistry));
        }

        public string Name => "help";

        public string Description => "Displays help information.";

        public string Group => "Built-in";

        public void Execute(string[] args)
        {
            Console.WriteLine("Available commands:");

            foreach (ICommand command in _commandRegistry.Commands)
            {
                Console.WriteLine(
                    $"- {command.Name}: {command.Description}");
            }
        }
    }
}