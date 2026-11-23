// This file is based on https://github.com/Tin-Systems-Platform/Tinos3/blob/main/Tinos3.Graphical/Desktop/WindowManager.cs
using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Tinos.Api.Common.Interfaces;

namespace Tinos.Api.Common.Graphics.Windows;

/// <summary>Manages window state, input routing, focus, z-order, and rendering for a graphical desktop.</summary>
public class WindowManager : IWindowManager
{
    private readonly List<Window> _windows;
    private readonly Dictionary<Window, IWindow> _windowSources;

    // Edge-triggered mouse state
    private bool _wasLeftButtonDown;
    private bool _isDragging;
    private Window? _inputCaptureWindow;
    private int _dragOffsetX;
    private int _dragOffsetY;
    private Window? _draggedWindow;

    /// <summary>Gets or sets the screen width used to constrain dragged windows.</summary>
    public int ScreenWidth { get; set; }
    /// <summary>Gets or sets the screen height used to constrain dragged windows.</summary>
    public int ScreenHeight { get; set; }

    /// <summary>Creates a window manager using the specified screen dimensions.</summary>
    /// <param name="screenWidth">The screen width in pixels.</param>
    /// <param name="screenHeight">The screen height in pixels.</param>
    public WindowManager(int screenWidth, int screenHeight)
    {
        _windows = new List<Window>();
        _windowSources = new Dictionary<Window, IWindow>();
        ScreenWidth = screenWidth;
        ScreenHeight = screenHeight;
    }

    /// <summary>Creates a window manager with zero screen dimensions.</summary>
    /// <remarks>Set <see cref="ScreenWidth"/> and <see cref="ScreenHeight"/> before handling window dragging.</remarks>
    public WindowManager() : this(0, 0)
    {
    }

    /// <inheritdoc />
    public IWindow? ActiveWindow => _windows.LastOrDefault(window => window.IsActive) is Window active
        ? GetSource(active)
        : (_windows.Count == 0 ? null : GetSource(_windows[_windows.Count - 1]));

    /// <inheritdoc />
    public IReadOnlyCollection<IWindow> OpenWindows => _windows.Select(GetSource).ToArray();

    /// <inheritdoc />
    public void OpenWindow(IWindow content)
    {
        if (content == null) throw new ArgumentNullException(nameof(content));
        Window? existingWindow = FindWindow(content);
        if (existingWindow != null)
        {
            SetActiveWindow(existingWindow);
            return;
        }
        Window window = content as Window ?? new Window(content.x, content.y, content.Width, content.Height, content.Title, content.Content);
        _windowSources[window] = content;
        AddWindow(window);
    }

    /// <inheritdoc />
    public void CloseActiveWindow()
    {
        IWindow? active = ActiveWindow;
        if (active != null) CloseWindow(active);
    }

    /// <inheritdoc />
    public void BringToFront(IWindow window)
    {
        if (window == null) return;
        Window? renderWindow = FindWindow(window);
        if (renderWindow != null) SetActiveWindow(renderWindow);
    }

    /// <inheritdoc />
    public void CloseWindow(IWindow window)
    {
        if (window == null) return;
        Window? renderWindow = FindWindow(window);
        if (renderWindow != null) RemoveWindow(renderWindow);
    }

    private IWindow GetSource(Window window)
    {
        return _windowSources.TryGetValue(window, out IWindow? source) ? source ?? window : window;
    }

    private Window? FindWindow(IWindow source)
    {
        foreach (Window window in _windows)
        {
            if (ReferenceEquals(GetSource(window), source)) return window;
        }
        return null;
    }

    private void SyncFromSources()
    {
        foreach (Window window in _windows)
        {
            IWindow source = GetSource(window);
            if (ReferenceEquals(source, window)) continue;
            bool changed = window.Title != source.Title || window.X != source.x || window.Y != source.y ||
                window.Width != source.Width || window.Height != source.Height || window.IsActive != source.IsActive ||
                !ReferenceEquals(window.Content, source.Content);
            window.Title = source.Title;
            window.X = source.x;
            window.Y = source.y;
            window.Width = source.Width;
            window.Height = source.Height;
            window.IsActive = source.IsActive;
            window.Content = source.Content;
            if (changed) window.IsDirty = true;
        }
    }

    private void SyncToSources()
    {
        foreach (Window window in _windows)
        {
            IWindow source = GetSource(window);
            if (ReferenceEquals(source, window)) continue;
            source.Title = window.Title;
            source.x = window.X;
            source.y = window.Y;
            source.Width = window.Width;
            source.Height = window.Height;
            source.IsActive = window.IsActive;
        }
    }

    /// <summary>Adds a renderable window and makes it active.</summary>
    /// <param name="window">The concrete window to add.</param>
    public void AddWindow(Window window)
    {
        if (window == null) throw new ArgumentNullException(nameof(window));
        if (_windows.Contains(window))
        {
            SetActiveWindow(window);
            return;
        }
        _windows.Add(window);
        SetActiveWindow(window);
    }

    /// <summary>Removes a renderable window and invalidates the remaining windows for redraw.</summary>
    /// <param name="window">The window to remove.</param>
    public void RemoveWindow(Window window)
    {
        if (window == null) return;
        IWindow source = GetSource(window);
        _windows.Remove(window);
        _windowSources.Remove(window);
        source.IsActive = false;
        if (ReferenceEquals(_inputCaptureWindow, window)) _inputCaptureWindow = null;
        if (ReferenceEquals(_draggedWindow, window))
        {
            _draggedWindow = null;
            _isDragging = false;
        }
        // Mark all remaining windows as dirty since the background behind them changed
        foreach (var w in _windows) w.IsDirty = true;
    }

    /// <summary>Determines whether a concrete window is currently managed.</summary>
    /// <param name="window">The window to find.</param>
    /// <returns><see langword="true"/> if the window is managed; otherwise, <see langword="false"/>.</returns>
    public bool ContainsWindow(Window window) => _windows.Contains(window);

    /// <summary>Makes the specified window active and moves it to the top of the z-order.</summary>
    /// <param name="window">The window to activate.</param>
    public void SetActiveWindow(Window window)
    {
        if (window == null || !_windows.Contains(window)) return;
        foreach (var w in _windows)
        {
            if (w.IsActive != (w == window))
            {
                w.IsActive = (w == window);
                w.IsDirty = true; // Focus change requires redraw
            }
        }
        // Move to end of list for correct Z-ordering (renders last = on top)
        _windows.Remove(window);
        _windows.Add(window);
        SyncToSources();
    }

    // put the current window to be inactive but keep it in the list for rendering
    /// <summary>Deactivates a window without removing it from the render order.</summary>
    /// <param name="window">The window to deactivate.</param>
    public void SetInactiveWindow(Window window)
    {
        if (window == null) return;
        if (window.IsActive)
        {
            window.IsActive = false;
            window.IsDirty = true; // Focus change requires redraw
        }
        SyncToSources();
    }

    /// <summary>Routes the current mouse state to window content and processes focus, close, and drag actions.</summary>
    public void HandleInput()
    {
        SyncFromSources();
        int mouseX = (int)MouseManager.X;
        int mouseY = (int)MouseManager.Y;
        bool isLeftButtonDown = MouseManager.LeftButton;

        // Capture the window where a press begins so widgets receive the
        // matching release even if the pointer moves outside the window.
        Window? inputWindow = null;
        if (isLeftButtonDown && !_wasLeftButtonDown)
        {
            for (int i = _windows.Count - 1; i >= 0; i--)
            {
                if (_windows[i].ContainsPoint(mouseX, mouseY))
                {
                    _inputCaptureWindow = _windows[i];
                    break;
                }
            }
        }

        if (_inputCaptureWindow != null && (isLeftButtonDown || _wasLeftButtonDown))
        {
            inputWindow = _inputCaptureWindow;
        }
        else
        {
            for (int i = _windows.Count - 1; i >= 0; i--)
            {
                if (_windows[i].ContainsPoint(mouseX, mouseY))
                {
                    inputWindow = _windows[i];
                    break;
                }
            }
        }

        inputWindow?.HandleContentInput(mouseX, mouseY, isLeftButtonDown);
        if (!isLeftButtonDown && _wasLeftButtonDown)
        {
            _inputCaptureWindow = null;
        }

        // 1. Detect Mouse Down (Rising Edge)
        if (isLeftButtonDown && !_wasLeftButtonDown)
        {
            // Check windows from top to bottom
            for (int i = _windows.Count - 1; i >= 0; i--)
            {
                Window w = _windows[i];

                // Check Close Button Click
                if (w.IsActive && w.IsCloseButtonHovered(mouseX, mouseY))
                {
                    RemoveWindow(w);
                    break; // Stop processing this click
                }

                if (w.IsInTitleBar(mouseX, mouseY))
                {
                    SetActiveWindow(w);
                    _isDragging = true;
                    _draggedWindow = w;
                    _dragOffsetX = mouseX - w.X;
                    _dragOffsetY = mouseY - w.Y;
                    break;
                }
                else if (w.ContainsPoint(mouseX, mouseY))
                {
                    SetActiveWindow(w);
                    break;
                }
            }
        }

        // 2. Stop dragging immediately when the left button is released.
        if (!isLeftButtonDown)
        {
            _isDragging = false;
            _draggedWindow = null;
        }

        // 3. Handle Dragging only while the left button is held.
        if (isLeftButtonDown && _isDragging && _draggedWindow != null)
        {
            _draggedWindow.X = mouseX - _dragOffsetX;
            _draggedWindow.Y = mouseY - _dragOffsetY;

            // Clamp to screen bounds (Architecture/Robustness)
            if (_draggedWindow.X < 0) _draggedWindow.X = 0;
            if (_draggedWindow.Y < 0) _draggedWindow.Y = 0;
            if (_draggedWindow.X + _draggedWindow.Width > ScreenWidth)
                _draggedWindow.X = Math.Max(0, ScreenWidth - _draggedWindow.Width);
            if (_draggedWindow.Y + _draggedWindow.Height > ScreenHeight)
                _draggedWindow.Y = Math.Max(0, ScreenHeight - _draggedWindow.Height);

            _draggedWindow.IsDirty = true; // Movement requires redraw
        }

        _wasLeftButtonDown = isLeftButtonDown;
        SyncToSources();
    }

    /// <summary>Routes a keyboard event to the active window, or the topmost window when none is active.</summary>
    /// <param name="keyInfo">The key event to route.</param>
    public void HandleKeyInput(ConsoleKeyInfo keyInfo)
    {
        SyncFromSources();
        Window? targetWindow = null;

        for (int i = _windows.Count - 1; i >= 0; i--)
        {
            if (_windows[i].IsActive)
            {
                targetWindow = _windows[i];
                break;
            }
        }

        if (targetWindow == null && _windows.Count > 0)
        {
            targetWindow = _windows[_windows.Count - 1];
        }

        targetWindow?.HandleContentKeyInput(keyInfo);
        SyncToSources();
    }

    /// <summary>Renders all managed windows from bottom to top onto the specified canvas.</summary>
    /// <param name="targetCanvas">The canvas to draw the windows on.</param>
    public void RenderAll(Canvas targetCanvas)
    {
        if (targetCanvas == null) throw new ArgumentNullException(nameof(targetCanvas));
        SyncFromSources();
        foreach (var window in _windows)
        {
            window.Render(targetCanvas);
        }
        SyncToSources();
    }
}
