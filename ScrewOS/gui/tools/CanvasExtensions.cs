using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.gui.tools
{
    internal static class CanvasExtensions
    {
        internal static void DrawImageAlphaDoubleBuffered(this Canvas canvas, Image image, int x, int y, bool preventOffBoundPixels = true)
        {
            int startX = 0, startY = 0;
            int endX = (int)image.Width;
            int endY = (int)image.Height;

            if (preventOffBoundPixels)
            {
                endX = (int)Math.Min(image.Width, canvas.Mode.Width - x);
                endY = (int)Math.Min(image.Height, canvas.Mode.Height - y);

                if (endX <= 0 || endY <= 0) return;
            }

            for (int i = startX; i < endX; i++)
            {
                for (int j = startY; j < endY; j++)
                {
                    Color color = Color.FromArgb(image.RawData[i + j * image.Width]);
                    canvas.DrawPoint(color, x + i, y + j);
                }
            }
        }

    }
}
