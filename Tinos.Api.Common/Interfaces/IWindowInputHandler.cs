using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos.Api.Common.Interfaces
{
    /// <summary>Optional input handling for window content that contains interactive widgets.</summary>
    public interface IWindowInputHandler
    {
        /// <summary>Receives mouse coordinates local to the window canvas.</summary>
        void HandleInput(int mouseX, int mouseY, bool leftButtonDown);
    }
}
