using System;
using System.Collections.Generic;
using System.Text;
using Tinos.Api.Common.Interfaces;

namespace Tinos.Api.Shell.Shell
{
    /// <summary>
    /// A registry for managing commands.
    /// </summary>
    public class CommandRegistry
    {
        private readonly List<ICommand> _commands = new();

        /// <summary>
        /// List of registered commands. This list is used to store all the commands that have been registered with the command registry.
        /// </summary>
        public IReadOnlyList<ICommand> Commands => _commands.AsReadOnly();


        /// <summary>
        /// Registers a command with the command registry.
        /// </summary>
        /// <param name="command">The command to register.</param>
        public void RegisterCommand(ICommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            _commands.Add(command);
        }
    }
}
