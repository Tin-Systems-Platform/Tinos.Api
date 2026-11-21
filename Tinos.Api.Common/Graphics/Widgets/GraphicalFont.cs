using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System;
using System.Drawing;
using System.IO;

namespace Tinos.Api.Common.Graphics.Widgets;

/// <summary>Shared custom font used by graphical text throughout Tinos3.</summary>
public static class GraphicalFont
{
    /// <summary>The default size used when drawing text with this font.</summary>
    public const int DefaultSize = 18;
    private static TrueTypeFont _font = null!;

    /// <summary>Gets the shared TrueType font, loading it from the system font paths on first access.</summary>
    public static TrueTypeFont Font
    {
        get
        {
            if (_font == null)
            {
                if (!File.Exists("/mnt/font.ttf"))
                {
                    try
                    {
                        File.Copy("/cd/font.ttf", "/mnt/font.ttf");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error copying font file: {ex.Message}");
                        throw;
                    }
                }

                _font = new TrueTypeFont("/mnt/font.ttf");
            }

            return _font;
        }
    }

    /// <summary>Draws non-empty text using the shared font at its default size.</summary>
    /// <param name="canvas">The canvas on which to draw the text.</param>
    /// <param name="text">The text to draw.</param>
    /// <param name="color">The text color.</param>
    /// <param name="x">The horizontal position of the text.</param>
    /// <param name="y">The vertical position of the text.</param>
    public static void DrawString(Canvas canvas, string text, Color color, int x, int y)
    {
        if (!string.IsNullOrEmpty(text))
        {
            canvas.DrawString(text, Font, DefaultSize, color, x, y);
        }
    }

    /// <summary>Measures text using the shared font at its default size.</summary>
    /// <param name="text">The text to measure.</param>
    /// <returns>The measured width, or zero when the text is null or empty.</returns>
    public static int MeasureString(string text)
    {
        return string.IsNullOrEmpty(text) ? 0 : Font.MeasureString(text, DefaultSize);
    }
}
