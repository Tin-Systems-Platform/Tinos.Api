using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos.Api.Examples.Graphics.Graphics
{
    public class GUI
    {

        private Canvas _mainCanvas;

        public void RunGUI()
        {
            _mainCanvas = Canvas.GetFullScreen();
            MouseManager.SetScreenSize(_mainCanvas.Width, _mainCanvas.Height);


        }
    }
}
