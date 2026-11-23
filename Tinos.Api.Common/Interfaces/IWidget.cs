using Cosmos.Kernel.System.Graphics;

namespace Tinos.Api.Common.Interfaces
{
    /// <summary>
    /// This interface is used to mark a class as a widget. Widgets are components that can be used by graphical apps.
    /// They can be used to display information, collect input, or perform actions.
    /// </summary>
    public interface IWidget
    {
        /// <summary>
        /// The X coordinate of the widget.
        /// </summary>
        int X { get; set; }

        /// <summary>
        /// The Y coordinate of the widget.
        /// </summary>
        int Y { get; set; }

        /// <summary>
        /// The width of the widget.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// The height of the widget.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Draws the widget onto the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw the widget onto.</param>
        void Draw(Canvas canvas);
    }
}