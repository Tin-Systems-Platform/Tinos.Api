using System;
using System.Collections.Generic;
using System.Text;
using Tinos.Api.Common.Graphics.Windows;

namespace Tinos.Api.Common.Graphics.Windows
{
    /// <summary>
    /// Window interface for the Tinos API. 
    /// This interface can be implemented by classes that represent windows in the OS, providing a contract for window behavior and properties.
    /// </summary>
    public interface IWindow
    {
        /// <summary>
        /// The window title. This property can be used to get or set the title of the window, which is typically displayed in the window's title bar.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Y value of the window
        /// </summary>
        int y { get; set; }
        /// <summary>
        /// X value of the window
        /// </summary>
        int x { get; set; }

        /// <summary>
        /// width of the window
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Height of the window
        /// </summary>
        int Height { get; set; }
        /// <summary>
        /// For checks of is the window active or not.
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// The window content.
        /// </summary>
        IWindowContent Content { get; set; }
    }
}
