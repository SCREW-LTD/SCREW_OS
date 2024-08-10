using Cobalt.GetIMG;
using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.System;
using Cosmos.System.Graphics;
using ScrewOS.gui.components;
using ScrewOS.gui.tools;
using ScrewOS.gui.utils.ttf;
using System;
using System.Drawing;
using System.Resources;

namespace ScrewOS.gui
{
    public class GuiHost
    {
        public const uint defaultScreenW = 1280, defaultScreenH = 720;

        public static SVGAIICanvas canvas;
        public static Bitmap cachedWindow;
        public static CGSSurface surface;
        private bool isInited = false;

        public static TTFFont RegularFont, OpenSansBold;
        public Image Cursor, Background;

        int frames = 0;
        public static int FPS = 0;
        int currentSecond = 0;

        public void Init(uint w = defaultScreenW, uint h = defaultScreenH)
        {
            MouseManager.ScreenWidth = w;
            MouseManager.ScreenHeight = h;

            MouseManager.X = w / 2;
            MouseManager.Y = h / 2;

            canvas = new SVGAIICanvas(new Mode(w, h, ColorDepth.ColorDepth32));
            Cosmos.Core.CPU.InitSSE();

            surface = new CGSSurface(canvas);

            RegularFont = new TTFFont(EmbeddedResourceLoader.LoadEmbeddedResource("FreeSans.ttf"));
            OpenSansBold = new TTFFont(EmbeddedResourceLoader.LoadEmbeddedResource("OpenSans-Bold.ttf"));

            Cursor = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("Cursor"));

            Bitmap wallpaper = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("Background"));

            Background = wallpaper.ResizeHeightKeepRatio(canvas.Mode.Height);

            isInited = true;
        }

        public void Run()
        {
            if (cachedWindow == null)
            {
                canvas.DrawImage(Background, 0, 0);
                cachedWindow = TakeBitmap.GetImage(0, 0, (int)canvas.Mode.Width, (int)canvas.Mode.Height);
            }
            else
            {
                canvas.DrawImage(cachedWindow, 0, 0);
            }
            
            StatusBar.Render();
            canvas.DrawImageAlpha(Cursor, (int)MouseManager.X, (int)MouseManager.Y);
            canvas.Display();

            frames++;
            if (RTC.Second != currentSecond)
            {
                currentSecond = RTC.Second;
                FPS = frames;
                frames = 0;
            }

            if (frames % 8 == 1)
            {
                Heap.Collect();
            }
        }
    }
}
