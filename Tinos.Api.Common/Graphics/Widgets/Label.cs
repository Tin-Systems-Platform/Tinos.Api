using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System.Drawing;

namespace Tinos.Api.Common.Graphics.Widgets
{
    /// <summary>Displays text at a fixed position on a canvas.</summary>
    public class Label
    {
        /// <summary>Gets or sets the horizontal position of the label.</summary>
        public int X { get; set; }

        /// <summary>Gets or sets the vertical position of the label.</summary>
        public int Y { get; set; }

        /// <summary>Gets or sets the text displayed by the label.</summary>
        public string Text { get; set; }

        /// <summary>Draws the label using the shared default font and black text.</summary>
        /// <param name="canvas">The canvas on which to draw the label.</param>
        public void Draw(Canvas canvas)
        {
            Draw(canvas, GraphicalFont.Font, Color.Black, GraphicalFont.DefaultSize);
        }

        /// <summary>Draws the label using the shared default font and specified text color.</summary>
        /// <param name="canvas">The canvas on which to draw the label.</param>
        /// <param name="color">The text color.</param>
        public void Draw(Canvas canvas, Color color)
        {
            if (Text == null)
            {
                return;
            }

            int lineStart = 0;
            int lineY = Y;

            for (int i = 0; i <= Text.Length; i++)
            {
                if (i < Text.Length && Text[i] != '\n')
                {
                    continue;
                }

                int lineLength = i - lineStart;
                // Treat Windows CRLF and Unix LF line endings identically.
                if (lineLength > 0 && Text[lineStart + lineLength - 1] == '\r')
                {
                    lineLength--;
                }

                if (lineLength > 0)
                {
                    string line = Text.Substring(lineStart, lineLength);
                    GraphicalFont.DrawString(canvas, line, color, X, lineY);
                }

                lineStart = i + 1;
                lineY += GraphicalFont.DefaultSize + 5;
            }
        }

        /// <summary>Draws the label using the specified font, color, and size.</summary>
        /// <param name="canvas">The canvas on which to draw the label.</param>
        /// <param name="font">The font used to render the text.</param>
        /// <param name="color">The text color.</param>
        /// <param name="size">The font size and line spacing in pixels.</param>
        public void Draw(Canvas canvas, TrueTypeFont font, Color color, int size)
        {
            if (Text == null)
            {
                return;
            }
            int lineStart = 0;
            int lineY = Y;
            for (int i = 0; i <= Text.Length; i++)
            {
                if (i < Text.Length && Text[i] != '\n')
                {
                    continue;
                }
                int lineLength = i - lineStart;
                // Treat Windows CRLF and Unix LF line endings identically.
                if (lineLength > 0 && Text[lineStart + lineLength - 1] == '\r')
                {
                    lineLength--;
                }
                if (lineLength > 0)
                {
                    string line = Text.Substring(lineStart, lineLength);
                    canvas.DrawString(line, font, size, color, X, lineY);
                }
                lineStart = i + 1;
                lineY += size + 5; // Adjust the vertical spacing based on the font size
            }
        }
    }
}
