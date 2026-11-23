using System;
using Cosmos.Kernel.System;
using Cosmos.Kernel.System.Graphics;
using System.Drawing;
using Tinos.Api.Common.Graphics.Widgets;
using Tinos.Api.Common.Interfaces;

namespace Tinos.Api.Common.Graphics.Windows;

/// <summary>A movable, renderable window with a title bar and optional content.</summary>
public class Window : IWindow
{
    /// <summary>Gets or sets the horizontal position of the window.</summary>
    public int X { get; set; }
    /// <summary>Gets or sets the vertical position of the window.</summary>
    public int Y { get; set; }
    /// <summary>Gets or sets the width of the window in pixels.</summary>
    public int Width { get; set; }
    /// <summary>Gets or sets the height of the window in pixels.</summary>
    public int Height { get; set; }
    /// <summary>Gets or sets the title displayed in the window title bar.</summary>
    public string Title { get; set; }
    /// <summary>Gets or sets whether this window is active.</summary>
    public bool IsActive { get; set; }
    /// <summary>Gets or sets the content rendered inside the window.</summary>
    public IWindowContent Content { get => _content; set { _content = value; IsDirty = true; } }

    // Efficiency: Only redraw the off-screen canvas when something changes
    /// <summary>Gets or sets whether the cached window rendering needs to be redrawn.</summary>
    public bool IsDirty { get; set; } = true;

    private readonly Canvas _offscreenCanvas;
    private readonly ButtonWidget _closeButton;

    private IWindowContent _content;

    string IWindow.Title { get => Title; set { Title = value; IsDirty = true; } }
    int IWindow.x { get => X; set { X = value; IsDirty = true; } }
    int IWindow.y { get => Y; set { Y = value; IsDirty = true; } }

    /// <summary>Creates a window with the specified bounds, title, and optional content.</summary>
    /// <param name="x">The horizontal position of the window.</param>
    /// <param name="y">The vertical position of the window.</param>
    /// <param name="width">The window width in pixels.</param>
    /// <param name="height">The window height in pixels.</param>
    /// <param name="title">The title displayed in the title bar.</param>
    /// <param name="windowContent">Optional content rendered inside the window.</param>
    public Window(int x, int y, int width, int height, string title, IWindowContent windowContent = null)
    {
        X = x; Y = y; Width = width; Height = height; Title = title;
        _offscreenCanvas = new Canvas(width, height);
        _content = windowContent;

        // Initialize Close Button (top right of title bar)
        _closeButton = new ButtonWidget
        {
            X = width - 30,
            Y = 4,
            Width = 26,
            Height = 26,
            Text = "X",
            OnClick = () => { /* Handled by WindowManager for removal */ }
        };
    }
    /// <summary>Redraws the window and its content to its off-screen canvas.</summary>
    public void DrawContent()
    {
        // Clear background
        _offscreenCanvas.Clear(IsActive ? Color.LightGray : Color.Silver);

        // Title Bar
        Color titleBarColor = IsActive ? Color.DarkBlue : Color.Gray;
        _offscreenCanvas.DrawFilledRectangle(titleBarColor, 0, 0, Width, 30);
        _offscreenCanvas.DrawLine(Color.Black, 0, 30, Width, 30);

        // Window Border
        _offscreenCanvas.DrawRectangle(Color.Black, 0, 0, Width, Height);

        // Title Text
        GraphicalFont.DrawString(_offscreenCanvas, Title, Color.White, 5, 3);

        // Draw Close Button
        _closeButton.Draw(_offscreenCanvas);

        //TODO: Add support for adding widgets managed by the application to the window and render them here. For now, we can just draw some example content.
        // Example Content
        _content?.DrawContent(_offscreenCanvas);

        IsDirty = false; // Mark as clean after drawing
    }

    /// <summary>Draws the window onto the specified canvas, refreshing its cached image if needed.</summary>
    /// <param name="targetCanvas">The canvas on which to render the window.</param>
    public void Render(Canvas targetCanvas)
    {
        if (IsDirty)
        {
            DrawContent();
        }
        // Blit the pre-rendered window to the main canvas
        targetCanvas.DrawCanvas(_offscreenCanvas, X, Y);
    }

    /// <summary>Determines whether a screen coordinate lies within the window bounds.</summary>
    /// <param name="x">The horizontal screen coordinate.</param>
    /// <param name="y">The vertical screen coordinate.</param>
    /// <returns><see langword="true"/> if the point is inside the window; otherwise, <see langword="false"/>.</returns>
    public bool ContainsPoint(int x, int y)
    {
        return x >= X && x <= X + Width && y >= Y && y <= Y + Height;
    }

    /// <summary>Determines whether a screen coordinate lies in the draggable title bar area.</summary>
    /// <param name="x">The horizontal screen coordinate.</param>
    /// <param name="y">The vertical screen coordinate.</param>
    /// <returns><see langword="true"/> if the point is in the title bar outside the close button area.</returns>
    public bool IsInTitleBar(int x, int y)
    {
        return ContainsPoint(x, y) && y <= Y + 25 && x <= X + Width - 25; // Exclude close button area
    }

    /// <summary>Determines whether the close button is hovered at the specified screen coordinate.</summary>
    /// <param name="x">The horizontal screen coordinate.</param>
    /// <param name="y">The vertical screen coordinate.</param>
    /// <returns><see langword="true"/> if the close button is hovered.</returns>
    public bool IsCloseButtonHovered(int x, int y)
    {
        return _closeButton.IsHovered(x - X, y - Y);
    }

    /// <summary>Routes mouse input to the window content when it supports input handling.</summary>
    /// <param name="mouseX">The horizontal screen coordinate of the mouse.</param>
    /// <param name="mouseY">The vertical screen coordinate of the mouse.</param>
    /// <param name="leftButtonDown">Whether the left mouse button is currently down.</param>
    public void HandleContentInput(int mouseX, int mouseY, bool leftButtonDown)
    {
        if (_content is IWindowInputHandler inputHandler)
        {
            inputHandler.HandleInput(mouseX - X, mouseY - Y, leftButtonDown);
            IsDirty = true;
        }
    }

    /// <summary>Routes a keyboard event to the window content when it supports keyboard input.</summary>
    /// <param name="keyInfo">The keyboard event to route.</param>
    public void HandleContentKeyInput(ConsoleKeyInfo keyInfo)
    {
        if (_content is IWindowKeyInputHandler keyInputHandler)
        {
            keyInputHandler.HandleKeyInput(keyInfo);
            IsDirty = true;
        }
    }

    /// <summary>Updates the window title and marks the cached rendering as needing refresh.</summary>
    /// <param name="title">The new title to display.</param>
    public void WindowTitle(string title)
    {
        Title = title;
        IsDirty = true;
    }

    // Blank constructor for cases where you want to use
    // the Window class without immediately setting properties or at all.
    /// <summary>Creates an uninitialized window whose properties can be set later.</summary>
    public Window() { }
}
