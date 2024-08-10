using Cosmos.Core;
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

        internal static void DrawRoundedRectangle(this Canvas canvas, int x, int y, int width, int height, int radius, Color col)
        {
            radius = Math.Min(radius, Math.Min(width / 2, height / 2));

            canvas.DrawFilledRectangle(col, x + radius, y + radius, width - 2 * radius, height - 2 * radius);

            canvas.DrawFilledRectangle(col, x + radius, y, width - 2 * radius, radius + 1);
            canvas.DrawFilledRectangle(col, x + radius, y + height - radius - 1, width - 2 * radius, radius + 2);

            canvas.DrawFilledRectangle(col, x, y + radius, radius + 1, height - 2 * radius); 
            canvas.DrawFilledRectangle(col, x + width - radius - 1, y + radius, radius + 1, height - 2 * radius);

            canvas.DrawFilledCircle(col, x + radius, y + radius, radius);
            canvas.DrawFilledCircle(col, x + width - radius, y + radius, radius);
            canvas.DrawFilledCircle(col, x + radius, y + height - radius, radius); 
            canvas.DrawFilledCircle(col, x + width - radius, y + height - radius, radius);
        }

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
