using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos.Api.Common.Interfaces
{
    /// <summary>Optional keyboard input handling for focused or active window content.</summary>
    public interface IWindowKeyInputHandler
    {
        /// <summary>Receives a keyboard event routed by the window manager.</summary>
        void HandleKeyInput(ConsoleKeyInfo keyInfo);
    }
}
