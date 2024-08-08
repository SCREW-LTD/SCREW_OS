using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.HAL.Drivers.Video.SVGAII;
using Cosmos.System;
using Cosmos.System.Graphics;
using ScrewOS.gui.tools;
using ScrewOS.gui.utils.ttf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScrewOS.gui
{
    public class GuiHost
    {
        public const uint defaultScreenW = 1920, defaultScreenH = 1080;
        static uint screenw, screenh;

        internal static VMWareSVGAII canvas;
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

            canvas = new VMWareSVGAII();
            canvas.SetMode(w, h);
            canvas.DoubleBufferUpdate();

            surface = new CGSSurface(canvas);
            RegularFont = new TTFFont(EmbeddedResourceLoader.LoadEmbeddedResource("FreeSans.ttf"));

            Cursor = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("Cursor"));

            Bitmap wallpaper = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("Background"));
            Background = wallpaper.Resize(screenw, screenh);

            canvas.DefineAlphaCursor(Cursor.Width, Cursor.Height, Cursor.RawData);

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
            RegularFont.DrawToSurface(surface, 16, 10, 64, $"RAM: {Cosmos.Core.GCImplementation.GetUsedRAM() / 1024 / 1024}/{Cosmos.Core.CPU.GetAmountOfRAM()} MB", Color.White);
            RegularFont.DrawToSurface(surface, 16, 10, 80, $"Press ESC to exit", Color.White);
        }

        static void Run()
        {
            canvas.Clear((uint)Color.Black.ToArgb());
            canvas.videoMemory.Copy((int)canvas.FrameSize, Background.RawData, 0, Background.RawData.Length);
            
            DrawDebug();

            canvas.SetCursor(true, MouseManager.X, MouseManager.Y);

            canvas.DoubleBufferUpdate();

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
