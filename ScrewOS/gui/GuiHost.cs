using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.System;
using Cosmos.System.Graphics;
using ScrewOS.gui.utils.ttf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.gui
{
    public class GuiHost
    {
        public const uint defaultScreenW = 640, defaultScreenH = 480;
        static uint screenw, screenh;
        
        internal static Canvas canvas;
        public static CGSSurface surface;

        public static TTFFont RegularFont;
        public static Image Cursor, Background;

        static int frames = 0;
        static int FPS = 0;
        static int currentSecond = 0;
        static int garbageCollected = 0;
        public static void Init(uint w = defaultScreenW, uint h = defaultScreenH)
        {
            screenw = w;
            screenh = h;
            MouseManager.ScreenWidth = w;
            MouseManager.ScreenHeight = h;
            MouseManager.X = w / 2;
            MouseManager.Y = h / 2;

            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode((int)w, (int)h, ColorDepth.ColorDepth32));
            canvas.Clear(Color.Black);
            surface = new CGSSurface(canvas);
            RegularFont = new TTFFont(EmbeddedResourceLoader.LoadEmbeddedResource("FreeSans.ttf"));
            Cursor = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("Cursor"));
            Background = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("Background"));
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
            RegularFont.DrawToSurface(surface, 16, 10, 32, $"FPS: {FPS}", Color.White);
            RegularFont.DrawToSurface(surface, 16, 10, 48, $"Mouse: X-{MouseManager.X} Y-{MouseManager.Y}", Color.White);
            RegularFont.DrawToSurface(surface, 16, 10, 64, $"Garbage Collected: {garbageCollected} objs", Color.White);
            RegularFont.DrawToSurface(surface, 16, 10, 80, $"Press ESC to exit", Color.White);
        }

        static void Run()
        {
            canvas.Clear(Color.Black);
            canvas.DrawImage(Background, 0, 0);

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

            garbageCollected += Heap.Collect();
        }
    }
}
