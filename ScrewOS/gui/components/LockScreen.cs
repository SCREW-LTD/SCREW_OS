using Cosmos.System;
using Cosmos.System.Graphics;
using ScrewOS.gui.tools;
using ScrewOS.gui.utils.ttf;
using System;
using System.Drawing;

namespace ScrewOS.gui.components
{
    public class LockScreen : GuiElementWithRerender
    {
        private const int LockScreenZIndex = 999;
        public int zIndex { get; } = LockScreenZIndex;

        private int yPos = 0;
        private int targetY = 0;
        private readonly int canvasWidth = (int)GuiHost.canvas.Mode.Width;
        private readonly int canvasHeight = (int)GuiHost.canvas.Mode.Height;

        private float elapsedTime = 0f;
        private readonly float duration = 7f;

        public void Render()
        {
            UpdatePosition(1f / GuiHost.FPS);
            UpdateGuiState();

            if (GuiHost.state == GuiState.LockScreen)
            {
                RenderLockScreen();
            }
        }

        private void UpdatePosition(float deltaTime)
        {
            if (yPos != targetY)
            {
                elapsedTime += deltaTime;
                float t = Math.Clamp(elapsedTime / duration, 0f, 1f);
                yPos = Lerp(yPos, targetY, t);

                if (t >= 1f)
                {
                    yPos = targetY;
                    elapsedTime = 0f;
                }
            }
        }

        private void UpdateGuiState()
        {
            if (MouseManager.MouseState == MouseState.Middle)
            {
                if (targetY == yPos)
                {
                    if (targetY == 0)
                        targetY = (int)GuiHost.canvas.Mode.Height;
                    else
                        targetY = 0;
                }
            }

            GuiHost.state = (yPos == (int)GuiHost.canvas.Mode.Height) ? GuiState.Desktop : GuiState.LockScreen;
        }

        private void RenderLockScreen()
        {
            GuiHost.canvas.DrawImage(GuiHost.cachedBlurWindow, 0, yPos);
            var hours = DateExecutor.ConvertTo12HourFormat(Cosmos.HAL.RTC.Hour);
            Element watches = GuiHost.OpenSansBold.DrawToSurface(
                GuiHost.surface,
                canvasHeight / 6,
                0,
                canvasWidth / 10 + yPos,
                $"{hours.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")}",
                Color.White,
                TTFFont.Alignment.Center,
                canvasWidth
            );

            Element date = GuiHost.OpenSansSemibold.DrawToSurface(
                GuiHost.surface,
                canvasHeight / 24,
                0,
                canvasWidth / 10 + yPos + watches.Height - 26,
                $"{Cosmos.HAL.RTC.DayOfTheMonth.ToString("D2")}.{Cosmos.HAL.RTC.Month.ToString("D2")}.{Cosmos.HAL.RTC.Year.ToString("D2")}",
                Color.White,
                TTFFont.Alignment.Center,
                canvasWidth
            );
        }

        public void ReRender()
        {
            // Implementation for ReRender
        }

        private static int Lerp(int start, int end, float t)
        {
            return (int)(start + (end - start) * t);
        }
    }
}
