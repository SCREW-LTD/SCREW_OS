using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.System;
using Cosmos.System.Graphics;
using ScrewOS.gui.tools;
using ScrewOS.gui.utils.ttf;
using System;
using System.Drawing;

namespace ScrewOS.gui
{
    public class GuiHost
    {
        public const uint defaultScreenW = 1280, defaultScreenH = 720;
        static uint screenw, screenh;

        internal static Canvas canvas;
        public static CGSSurface surface;
        
        public static TTFFont RegularFont;
        public static Image Cursor, Background;

        static int frames = 0;
        static int FPS = 0;
        static int currentSecond = 0;

        static Bitmap statusBar;
        public static void Init(uint w = defaultScreenW, uint h = defaultScreenH)
        {
            screenw = w;
            screenh = h;

            MouseManager.ScreenWidth = w;
            MouseManager.ScreenHeight = h;

            MouseManager.X = w / 2;
            MouseManager.Y = h / 2;

            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(w, h, ColorDepth.ColorDepth32));
            canvas.Clear(Color.Black);
            surface = new CGSSurface(canvas);

            canvas.Display();

            RegularFont = new TTFFont(EmbeddedResourceLoader.LoadEmbeddedResource("FreeSans.ttf"));

            Cursor = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("Cursor"));

            Bitmap wallpaper = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("Background"));
            Background = wallpaper.ResizeHeightKeepRatio(screenh);
            statusBar = BitmapCreator.CreateBitmapFromColor(Color.FromArgb(255, 30, 30, 30), screenw, 32, ColorDepth.ColorDepth32);

            Cosmos.Core.CPU.InitSSE();

            while (true)
            {
                Run();
                if (KeyboardManager.TryReadKey(out KeyEvent key) && key.Key == ConsoleKeyEx.Escape)
                {
                    Cosmos.System.Power.Reboot();
                    break;
                }
            }
        }

        static void DrawDebug()
        {
            RegularFont.DrawToSurface(surface, 16, 12, 21, $"FPS: {FPS}", Color.White);
            RegularFont.DrawToSurface(surface, 16, 12, 21, $"{Cosmos.Core.GCImplementation.GetUsedRAM() / 1024 / 1024}/{Cosmos.Core.CPU.GetAmountOfRAM()} MB", Color.White, TTFFont.Alignment.Right, (int)screenw);
            RegularFont.DrawToSurface(surface, 16, 0, 21, $"{Cosmos.HAL.RTC.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")}:{Cosmos.HAL.RTC.Second.ToString("D2")}", Color.White, TTFFont.Alignment.Center, (int)screenw);
        }

        private static bool isMouseDown = false;
        private static uint startX, startY;
        static void Run()
        {
            canvas.Clear(Color.Black);

            canvas.DrawImage(Background, 0, 0);
            canvas.DrawImage(statusBar, 0, 0);

            DrawDebug();

            canvas.DrawImageAlpha(Cursor, (int)MouseManager.X, (int)MouseManager.Y);

            canvas.Display();

            frames++;
            if (RTC.Second != currentSecond)
            {
                currentSecond = RTC.Second;
                FPS = frames;
                frames = 0;
            }

            if (frames % 5 == 1)
            {
                Heap.Collect();
            }
        }
    }
}
