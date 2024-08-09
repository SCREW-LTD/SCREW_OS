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
        public static void Render(GuiHost gui)
        {
            gui.canvas.DrawFilledRectangle(Color.FromArgb(180, 30, 30, 30), 0, 0, (int)gui.canvas.Mode.Width, 32);
            var hours = DateExecutor.ConvertTo12HourFormat(Cosmos.HAL.RTC.Hour);
            gui.OpenSansBold.DrawToSurface(gui.surface, 16, 0, 20, $"{DateExecutor.GetMonthAbbreviation(Cosmos.HAL.RTC.Month)} {Cosmos.HAL.RTC.DayOfTheMonth.ToString("D2")}  {hours.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")} {hours.Period}", Color.White, TTFFont.Alignment.Center, (int)gui.canvas.Mode.Width);
            gui.OpenSansBold.DrawToSurface(gui.surface, 16, 12, 20, $"FPS: {gui.FPS}", Color.White);
        }
    }
}
