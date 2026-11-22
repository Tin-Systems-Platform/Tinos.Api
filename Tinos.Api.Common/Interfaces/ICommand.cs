namespace Tinos.Api.Common.Interfaces
{
    /// <summary>
    /// Represents a command that can be executed.
    /// </summary>
    public interface ICommand
    {
        ///<summary>
        /// Gets the name of the command.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The description of the command, which can be used for help or documentation purposes.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The group or category to which the command belongs. This can be used to organize commands into logical groups for better discoverability and usability.
        /// This should be also be used to group commands in the help output, so that users can easily find related commands.
        /// </summary>
        string Group { get; }

        /// <summary>
        /// Executes the command.
        /// </summary>
        void Execute(string[] args);
    }
}