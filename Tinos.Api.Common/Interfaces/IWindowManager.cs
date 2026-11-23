using System;
using System.Collections.Generic;
using System.Text;
using Tinos.Api.Common.Graphics.Windows;
using Tinos.Api.Common.Interfaces;

namespace Tinos.Api.Common.Interfaces
{
    /// <summary>Represents a manager for handling and organizing windows in the application.</summary>
    public interface IWindowManager
    {
        /// <summary>Opens a new window with the specified content.</summary>
        /// <param name="content">The content to be displayed in the window.</param>
        void OpenWindow(IWindow content);
        /// <summary>Closes the currently active window.</summary>
        void CloseActiveWindow();
        /// <summary>Brings the specified window to the front, making it the active window.</summary>
        /// <param name="window">The window to bring to the front.</param>
        void BringToFront(IWindow window);

        /// <summary>
        /// Closes the specified window and removes it from the window manager's list of open windows.
        /// </summary>
        /// <param name="window">The window to close.</param>
        void CloseWindow(IWindow window);

        /// <summary>
        /// Gets the currently active window.
        /// </summary>
        IWindow? ActiveWindow { get; }

        /// <summary>
        /// Gets all open windows
        /// </summary>
        IReadOnlyCollection<IWindow> OpenWindows { get; }
    }
}
