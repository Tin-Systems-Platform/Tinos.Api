using Cosmos.Kernel.System.Graphics;
using System;
using System.Drawing;

namespace Tinos.Api.Common.Graphics.Widgets
{
    /// <summary>A single-line text input control that accepts keyboard input while focused.</summary>
    public class TextBox
    {
        private const int HorizontalPadding = 5;
        private const int GlyphWidth = 10;
        private bool _wasPressed;

        /// <summary>Gets or sets the horizontal position of the text box.</summary>
        public int X { get; set; }

        /// <summary>Gets or sets the vertical position of the text box.</summary>
        public int Y { get; set; }

        /// <summary>Gets or sets the width of the text box in pixels.</summary>
        public int Width { get; set; }

        /// <summary>Gets or sets the height of the text box in pixels.</summary>
        public int Height { get; set; }

        /// <summary>Gets whether the text box currently receives keyboard input.</summary>
        public bool IsFocused { get; private set; }

        /// <summary>Gets the current text entered in the text box.</summary>
        public string Text { get; private set; } = string.Empty;

        /// <summary>Determines whether a point in canvas coordinates lies within the text box.</summary>
        /// <param name="mouseX">The horizontal coordinate to check.</param>
        /// <param name="mouseY">The vertical coordinate to check.</param>
        /// <returns><see langword="true"/> if the point lies within the text box bounds.</returns>
        public bool IsHovered(int mouseX, int mouseY)
        {
            return mouseX >= X && mouseX <= X + Width && mouseY >= Y && mouseY <= Y + Height;
        }

        /// <summary>Updates focus based on a left mouse button press.</summary>
        /// <param name="mouseX">The horizontal mouse coordinate in canvas space.</param>
        /// <param name="mouseY">The vertical mouse coordinate in canvas space.</param>
        /// <param name="leftButtonDown">Whether the left mouse button is currently down.</param>
        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            bool isHovered = IsHovered(mouseX, mouseY);

            if (leftButtonDown && !_wasPressed)
            {
                IsFocused = isHovered;
            }

            _wasPressed = leftButtonDown;
        }

        /// <summary>Edits the text in response to a keyboard event when the text box is focused.</summary>
        /// <param name="keyInfo">The keyboard event to process.</param>
        public void HandleInput(ConsoleKeyInfo keyInfo)
        {
            if (!IsFocused)
            {
                return;
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (Text.Length > 0)
                {
                    Text = Text[..^1];
                }

                return;
            }

            if (!char.IsControl(keyInfo.KeyChar) && Text.Length < GetVisibleCharacterCapacity())
            {
                Text += keyInfo.KeyChar;
            }
        }

        /// <summary>Sets the text, truncating it to the number of characters visible in the text box.</summary>
        /// <param name="text">The text to display; null is treated as an empty string.</param>
        public void SetText(string text)
        {
            Text = text ?? string.Empty;
            if (Text.Length > GetVisibleCharacterCapacity())
            {
                Text = Text[..GetVisibleCharacterCapacity()];
            }
        }

        /// <summary>Draws the text box and its current text.</summary>
        /// <param name="canvas">The canvas on which to draw the text box.</param>
        public void Draw(Canvas canvas)
        {
            canvas.DrawFilledRectangle(Color.White, X, Y, Width, Height);
            canvas.DrawRectangle(IsFocused ? Color.Blue : Color.Black, X, Y, Width, Height);

            int textX = X + HorizontalPadding;
            int textY = Y + ((Height - GraphicalFont.DefaultSize) / 2);
            GraphicalFont.DrawString(canvas, Text, Color.Black, textX, textY);
        }

        /// <summary>Calculates how many fixed-width characters fit inside the text box.</summary>
        /// <returns>The number of characters that fit in the available inner width.</returns>
        private int GetVisibleCharacterCapacity()
        {
            int availableWidth = Math.Max(0, Width - (HorizontalPadding * 2));
            return Math.Max(0, availableWidth / GlyphWidth);
        }
    }
}
