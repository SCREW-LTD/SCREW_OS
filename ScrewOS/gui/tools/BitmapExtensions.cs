using Cosmos.System.Graphics;
using System;
using System.Drawing;

namespace ScrewOS.gui.tools
{
    internal static class BitmapExtensions
    {
        internal static Bitmap Resize(this Bitmap bmp, uint width, uint height)
        {
            if (bmp.Width == width && bmp.Height == height)
            {
                return bmp;
            }

            if (bmp.Depth != ColorDepth.ColorDepth32)
            {
                throw new Exception("Resize can only resize images with a colour depth of 32.");
            }

            Bitmap res = new Bitmap(width, height, ColorDepth.ColorDepth32);

            for (uint y = 0; y < height; y++)
            {
                for (uint x = 0; x < width; x++)
                {
                    double xRatio = (double)(bmp.Width - 1) / (double)width;
                    double yRatio = (double)(bmp.Height - 1) / (double)height;

                    double xOrig = x * xRatio;
                    double yOrig = y * yRatio;

                    uint xL = (uint)Math.Floor(xOrig);
                    uint xR = (uint)Math.Ceiling(xOrig);
                    uint yT = (uint)Math.Floor(yOrig);
                    uint yB = (uint)Math.Ceiling(yOrig);

                    var topLeft = bmp.RawData[yT * bmp.Width + xL];
                    var topRight = bmp.RawData[yT * bmp.Width + xR];
                    var bottomLeft = bmp.RawData[yB * bmp.Width + xL];
                    var bottomRight = bmp.RawData[yB * bmp.Width + xR];

                    double xLerp = xOrig - xL;
                    double yLerp = yOrig - yT;

                    uint top = LerpColor((uint)topLeft, (uint)topRight, xLerp);
                    uint bottom = LerpColor((uint)bottomLeft, (uint)bottomRight, xLerp);

                    res.RawData[y * width + x] = (int)LerpColor(top, bottom, yLerp);
                }
            }

            return res;
        }

        private static uint LerpColor(uint colorA, uint colorB, double t)
        {
            byte aA = (byte)((colorA >> 24) & 0xFF);
            byte rA = (byte)((colorA >> 16) & 0xFF);
            byte gA = (byte)((colorA >> 8) & 0xFF);
            byte bA = (byte)(colorA & 0xFF);

            byte aB = (byte)((colorB >> 24) & 0xFF);
            byte rB = (byte)((colorB >> 16) & 0xFF);
            byte gB = (byte)((colorB >> 8) & 0xFF);
            byte bB = (byte)(colorB & 0xFF);

            byte a = (byte)(aA + (aB - aA) * t);
            byte r = (byte)(rA + (rB - rA) * t);
            byte g = (byte)(gA + (gB - gA) * t);
            byte b = (byte)(bA + (bB - bA) * t);

            return (uint)((a << 24) | (r << 16) | (g << 8) | b);
        }

        internal static Bitmap ResizeWidthKeepRatio(this Bitmap bmp, uint width)
        {
            return Resize(bmp, width, (uint)((double)bmp.Height * ((double)width / (double)bmp.Width)));
        }

        internal static Bitmap ResizeHeightKeepRatio(this Bitmap bmp, uint height)
        {
            return Resize(bmp, (uint)((double)bmp.Width * ((double)height / (double)bmp.Height)), height);
        }

        internal static Bitmap SetOpacity(this Bitmap bmp, byte opacity)
        {
            if (bmp.Depth != ColorDepth.ColorDepth32)
            {
                throw new Exception("SetOpacity can only modify images with a colour depth of 32.");
            }

            Bitmap result = new Bitmap(bmp.Width, bmp.Height, ColorDepth.ColorDepth32);

            for (uint y = 0; y < bmp.Height; y++)
            {
                for (uint x = 0; x < bmp.Width; x++)
                {
                    int color = bmp.RawData[y * bmp.Width + x];
                    byte r = (byte)((color >> 16) & 0xFF);
                    byte g = (byte)((color >> 8) & 0xFF);
                    byte b = (byte)(color & 0xFF);

                    uint newColor = (uint)((opacity << 24) | (r << 16) | (g << 8) | b);
                    result.RawData[y * bmp.Width + x] = (int)newColor;
                }
            }

            return result;
        }
    }

    public static class BitmapCreator
    {
        public static Bitmap CreateBitmapFromColor(Color color, uint width, uint height, ColorDepth colorDepth)
        {
            if (width == 0 || height == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Width and height must be greater than 0.");
            }

            if (colorDepth != ColorDepth.ColorDepth24 && colorDepth != ColorDepth.ColorDepth32)
            {
                throw new NotSupportedException("Only ColorDepth24 and ColorDepth32 are supported.");
            }

            var bmp = new Bitmap(width, height, colorDepth)
            {
                RawData = new int[width * height]
            };

            int colorValue;
            if (colorDepth == ColorDepth.ColorDepth32)
            {
                colorValue = (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
            }
            else
            {
                colorValue = (color.R << 16) | (color.G << 8) | color.B;
            }

            for (int i = 0; i < bmp.RawData.Length; i++)
            {
                bmp.RawData[i] = colorValue;
            }

            return bmp;
        }
    }
}
