using System;
using System.Collections.Generic;
using System.Text;
using Tinos.Api.Common.Interfaces;
using Tinos.Api.Shell.Shell;
using System.Linq;
using Tinos.Api.Shell.BuiltIn;

namespace Tinos.Api.Shell
{
    /// <summary>
    /// A command-line shell for executing commands.
    /// </summary>
    public class CommandlineShell
    {
        private readonly CommandRegistry _commandRegistry;

        public CommandlineShell(CommandRegistry commandRegistry)
        {
            _commandRegistry = commandRegistry ?? throw new ArgumentNullException(nameof(commandRegistry));
        }

        public void ExecuteShell(string name)
        {
            HelpCmd helpCommand = new HelpCmd(_commandRegistry);
            _commandRegistry.RegisterCommand(helpCommand);
            Console.WriteLine($"Welcome to the {name} shell!");
            Console.WriteLine("Type 'help' to see available commands.");
            Console.WriteLine();

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                string commandName = parts[0];
                ICommand command = _commandRegistry.Commands.FirstOrDefault(c => c.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));

                if (command != null)
                {
                    try
                    {
                        command.Execute(parts.Skip(1).ToArray());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error executing command '{commandName}': {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown command: {commandName}");
                }
            }
        }
    }
}
