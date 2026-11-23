using System;
using Cosmos.Kernel.System;
using Cosmos.Kernel.System.Graphics;
using System.Drawing;
using Tinos.Api.Common.Interfaces;

namespace Tinos.Api.Common.Graphics.Widgets;

/// <summary>A rectangular button that tracks pointer hover and press state. Extends the IWidget interface.</summary>
public class ButtonWidget : IWidget
{
    /// <summary>Gets or sets the button's horizontal position on its canvas.</summary>
    public int X { get; set; }
    /// <summary>Gets or sets the button's vertical position on its canvas.</summary>
    public int Y { get; set; }
    /// <summary>Gets or sets the button width in pixels.</summary>
    public int Width { get; set; }
    /// <summary>Gets or sets the button height in pixels.</summary>
    public int Height { get; set; }
    /// <summary>Gets or sets the text displayed on the button.</summary>
    public string Text { get; set; }
    /// <summary>Gets or sets the action invoked when the button is clicked.</summary>
    public Action OnClick { get; set; }
    /// <summary>Gets whether the pointer is currently over the button.</summary>
    public bool IsHoveredState { get; private set; }
    /// <summary>Gets whether the pointer is currently pressing the button.</summary>
    public bool IsPressed { get; private set; }
    private bool _wasPressed;

    /// <summary>Determines whether a point in canvas coordinates lies within the button.</summary>
    /// <param name="mouseX">The horizontal coordinate to check.</param>
    /// <param name="mouseY">The vertical coordinate to check.</param>
    /// <returns><see langword="true"/> if the point lies within the button bounds.</returns>
    public bool IsHovered(int mouseX, int mouseY)
    {
        return mouseX >= X && mouseX <= X + Width && mouseY >= Y && mouseY <= Y + Height;
    }

    /// <summary>Updates hover and click state from coordinates local to the window canvas.</summary>
    public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
    {
        IsHoveredState = IsHovered(mouseX, mouseY);
        IsPressed = leftButtonDown && IsHoveredState;

        if (_wasPressed && !leftButtonDown && IsHoveredState)
        {
            OnClick?.Invoke();
        }

        _wasPressed = leftButtonDown && IsHoveredState;
    }

    /// <summary>Draws the button using its current hover and pressed state.</summary>
    /// <param name="canvas">The canvas on which to draw the button.</param>
    public void Draw(Canvas canvas)
    {
        Color bgColor = IsPressed ? Color.DarkGray : (IsHoveredState ? Color.LightGray : Color.White);
        canvas.DrawFilledRectangle(bgColor, X, Y, Width, Height);
        canvas.DrawRectangle(Color.Black, X, Y, Width, Height);

        int textX = X + (Width - (Text?.Length ?? 0) * 10) / 2;
        int textY = Y + ((Height - GraphicalFont.DefaultSize) / 2);
        GraphicalFont.DrawString(canvas, Text ?? string.Empty, Color.Black, textX, textY);
    }

    /// <summary>Draws the button, using the specified background color while pressed.</summary>
    /// <param name="canvas">The canvas on which to draw the button.</param>
    /// <param name="bgColor1">The background color to use while the button is pressed.</param>
    public void Draw(Canvas canvas, Color bgColor1)
    {
        Color bgColor = IsPressed ? bgColor1 : (IsHoveredState ? Color.LightGray : Color.White);
        canvas.DrawFilledRectangle(bgColor, X, Y, Width, Height);
        canvas.DrawRectangle(Color.Black, X, Y, Width, Height);
        int textX = X + (Width - (Text?.Length ?? 0) * 10) / 2;
        int textY = Y + ((Height - GraphicalFont.DefaultSize) / 2);
        GraphicalFont.DrawString(canvas, Text ?? string.Empty, Color.Black, textX, textY);
    }

}
