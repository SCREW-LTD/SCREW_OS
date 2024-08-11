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
using System.Linq;
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

        public static TTFFont RegularFont, OpenSansBold, OpenSansSemibold;
        public Bitmap Cursor, Background;

        int frames = 0;
        public static int FPS = 0;
        int currentSecond = 0;

        public static GuiState state = GuiState.LockScreen;
        private static List<GuiElement> guiElements = new List<GuiElement>();

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
            OpenSansSemibold = new TTFFont(EmbeddedResourceLoader.LoadEmbeddedResource("OpenSans-Semibold.ttf"));

            RenderWallpaper("Background2");

            Cursor = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource("Cursor"));
            RunProcess(new LockScreen());
            RunProcess(new StatusBar());
            
            isInited = true;
        }

        public void RenderWallpaper(string name)
        {
            Bitmap wallpaper = new Bitmap(EmbeddedResourceLoader.LoadEmbeddedResource(name));
            Background = wallpaper.ResizeHeightKeepRatio(canvas.Mode.Height);
            cachedWindow = null;
        }

        public static void RunProcess(GuiElement element)
        {
            guiElements.Add(element);

            for (int i = 1; i < guiElements.Count; i++)
            {
                var current = guiElements[i];
                int j = i - 1;

                while (j >= 0 && guiElements[j].zIndex > current.zIndex)
                {
                    guiElements[j + 1] = guiElements[j];
                    j--;
                }
                guiElements[j + 1] = current;
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
                canvas.DrawImage(cachedWindow, 0, 0);
            }

            foreach (var element in guiElements)
            {
                element.Render();

                if (element is IWindow window)
                {
                    window.DragMove();
                }
            }

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
