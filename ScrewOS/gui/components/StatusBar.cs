using Cobalt.GetIMG;
using Cosmos.System.Graphics;
using ScrewOS.gui.tools;
using ScrewOS.gui.utils.ttf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.gui.components
{
    public class StatusBar
    {
        private static Bitmap cachedStatusbar;
        private static Bitmap logo = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("Logo")).ResizeWidthKeepRatio(20);
        public static void Render()
        {
            if (cachedStatusbar == null)
            {
                Bitmap bpm = BitmapCreator.CreateBitmapFromColor(Color.FromArgb(200, 30, 30, 30), GuiHost.canvas.Mode.Width, 32, ColorDepth.ColorDepth32);
                GuiHost.canvas.DrawImageAlpha(bpm, 0, 0);

                cachedStatusbar = PostProcess.ApplyBlur(TakeBitmap.GetImage(0, 0, (int)GuiHost.canvas.Mode.Width, 32), 16);
            }
            else
            {
                GuiHost.canvas.DrawImage(cachedStatusbar, 0, 0);
                GuiHost.canvas.DrawRoundedRectangle(10, 11, 26, 8, 8, Color.White);
            }

            var hours = DateExecutor.ConvertTo12HourFormat(Cosmos.HAL.RTC.Hour);
            GuiHost.OpenSansBold.DrawToSurface(GuiHost.surface, 16, 0, 20, $"{DateExecutor.GetMonthAbbreviation(Cosmos.HAL.RTC.Month)} {Cosmos.HAL.RTC.DayOfTheMonth.ToString("D2")}  {hours.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")} {hours.Period}", Color.White, TTFFont.Alignment.Center, (int)GuiHost.canvas.Mode.Width);
            GuiHost.OpenSansBold.DrawToSurface(GuiHost.surface, 16, 12, 20, $"{GuiHost.FPS}", Color.White, TTFFont.Alignment.Right, (int)GuiHost.canvas.Mode.Width);
        }
    }
}
