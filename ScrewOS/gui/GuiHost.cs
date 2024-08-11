using Cobalt.GetIMG;
using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.System;
using Cosmos.System.Graphics;
using ScrewOS.gui.components;
using ScrewOS.gui.tasks;
using ScrewOS.gui.tasks.interfaces;
using ScrewOS.gui.tools;
using ScrewOS.gui.utils.ttf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;

namespace ScrewOS.gui
{
    public enum GuiState
    {
        LockScreen,
        Desktop
    }
    public class GuiHost
    {
        public const uint defaultScreenW = 1280, defaultScreenH = 720;

        public static SVGAIICanvas canvas;
        public static Bitmap cachedWindow, cachedBlurWindow;
        public static CGSSurface surface;
        private bool isInited = false;

        public static TTFFont RegularFont, OpenSansBold, OpenSans;
        public Image Cursor, Background;

        int frames = 0;
        public static int FPS = 0;
        int currentSecond = 0;

        GuiState state = GuiState.LockScreen;
        public List<GuiElement> guiElements = new List<GuiElement>();

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
            OpenSans = new TTFFont(EmbeddedResourceLoader.LoadEmbeddedResource("OpenSans.ttf"));

            RenderWallpaper("Background2");

            Cursor = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("Cursor"));

            guiElements.Add(new StatusBar());

            isInited = true;
        }

        public void RenderWallpaper(string name)
        {
            Bitmap wallpaper = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource(name));
            Background = wallpaper.ResizeHeightKeepRatio(canvas.Mode.Height);
            cachedWindow = null;
        }

        public void RenderDesktop()
        {
            foreach (var element in guiElements)
            {
                element.Render();

                if (element is IWindow window)
                {
                    window.DragMove();
                }
            }
        }

        public void RenderLockScreen()
        {
            canvas.DrawImage(cachedBlurWindow, 0, 0);
            var hours = DateExecutor.ConvertTo12HourFormat(Cosmos.HAL.RTC.Hour);
            OpenSansBold.DrawToSurface(surface, (int)canvas.Mode.Height / 6, 0, (int)canvas.Mode.Width / 10, $"{hours.Hour.ToString("D2")}:{Cosmos.HAL.RTC.Minute.ToString("D2")}", Color.White, TTFFont.Alignment.Center, (int)canvas.Mode.Width);
            if (MouseManager.MouseState == MouseState.Left)
            {
                state = GuiState.Desktop;
            }
        }

        public void CacheImages()
        {
            canvas.DrawImage(Background, 0, 0);
            cachedWindow = TakeBitmap.GetImage(0, 0, (int)canvas.Mode.Width, (int)canvas.Mode.Height);
            cachedBlurWindow = PostProcess.ApplyBlur(PostProcess.DarkenBitmap(cachedWindow, 0.8f), 20);
        }

        public void Run()
        {
            if (cachedWindow == null)
            {
                CacheImages();
            }
            else
            {
                if (state == GuiState.Desktop)
                    canvas.DrawImage(cachedWindow, 0, 0);
            }

            RenderDesktop();

            if(state == GuiState.LockScreen)
                RenderLockScreen();

            canvas.DrawImageAlpha(Cursor, (int)MouseManager.X, (int)MouseManager.Y);
            canvas.Display();

            frames++;
            if (RTC.Second != currentSecond)
            {
                currentSecond = RTC.Second;
                FPS = frames;
                frames = 0;
            }

            HeapCollector.Collect();
        }
    }
}
