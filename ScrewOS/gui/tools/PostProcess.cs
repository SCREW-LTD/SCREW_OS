using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewOS.gui.tools
{
    public class PostProcess
    {
        public static int[] DarkenBitmap(int[] data, float factor)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (255 << 24) | ((byte)(((data[i] >> 16) & 0xFF) * factor) << 16) | ((byte)(((data[i] >> 8) & 0xFF) * factor) << 8) | (byte)((data[i] & 0xFF) * factor);
            }
            return data;
        }

        public static int[] ResizeBitmap(Bitmap bmp, uint nX, uint nY, bool check = false)
        {
            if (check && bmp.Width == nX && bmp.Height == nY) { return bmp.RawData; }
            int[] result = new int[nX * nY];
            if (bmp.Width == nX && bmp.Height == nY) { result = bmp.RawData; return result; }

            for (int i = 0; i < nX; i++)
            {
                for (int j = 0; j < nY; j++)
                {
                    result[i + j * nX] = bmp.RawData[(i * bmp.Width / nX) + (j * bmp.Height / nY) * bmp.Width];
                }
            }
            return result;
        }
        public static int[] ResizeBitmap(int[] data, uint x, uint y, uint nX, uint nY)
        {
            int[] result = new int[nX * nY];
            if (x == nX && y == nY) { result = data; return result; }

            for (int i = 0; i < nX; i++)
            {
                for (int j = 0; j < nY; j++)
                {
                    result[i + j * nX] = data[(i * x / nX) + (j * y / nY) * x];
                }
            }
            return result;
        }
        public static Bitmap CropBitmap(Bitmap bitmap, int x, int y, int width, int height)
        {
            Bitmap croppedBitmap = new((uint)width, (uint)height, ColorDepth.ColorDepth32);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    croppedBitmap.RawData[row * width + col] = bitmap.RawData[(y + row) * bitmap.Width + (x + col)];
                }
            }
            return croppedBitmap;
        }

        public static int[] ApplyBlur(int[] originalData, int blurRadius, uint width = 640, uint height = 360)
        {
            int[] blurredData = new int[width * height];
            double[] kernel = new double[blurRadius * 2 + 1];
            double sum = 0;
            for (int i = 0; i < kernel.Length; i++)
            {
                kernel[i] = Math.Exp(-(Math.Pow(i - blurRadius, 2) / (2 * Math.Pow(blurRadius / 3.0, 2))));
                sum += kernel[i];
            }
            for (int i = 0; i < kernel.Length; i++) { kernel[i] /= sum; }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double r = 0, g = 0, b = 0;
                    for (int i = -blurRadius; i <= blurRadius; i++)
                    {
                        int idx = x + i;
                        if (idx < 0 || idx >= width)
                        {
                            idx = x;
                        }
                        int pixel = originalData[y * width + idx];
                        double weight = kernel[i + blurRadius];
                        r += ((pixel >> 16) & 0xFF) * weight;
                        g += ((pixel >> 8) & 0xFF) * weight;
                        b += (pixel & 0xFF) * weight;
                    }
                    blurredData[y * width + x] = ((int)r << 16) | ((int)g << 8) | (int)b;
                }
            }

            return blurredData;
        }
        public static Bitmap ApplyBlur(Bitmap originalBitmap, int blurRadius)
        {
            uint width = originalBitmap.Width;
            uint height = originalBitmap.Height;

            Bitmap blurredBitmap = new Bitmap(width, height, originalBitmap.Depth);
            int[] blurredData = new int[width * height];
            double[] kernel = new double[blurRadius * 2 + 1];
            double sum = 0;

            // Generate Gaussian kernel
            for (int i = 0; i < kernel.Length; i++)
            {
                kernel[i] = Math.Exp(-(Math.Pow(i - blurRadius, 2) / (2 * Math.Pow(blurRadius / 3.0, 2))));
                sum += kernel[i];
            }

            // Normalize the kernel
            for (int i = 0; i < kernel.Length; i++)
            {
                kernel[i] /= sum;
            }

            // Apply the blur horizontally
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double r = 0, g = 0, b = 0, a = 0;

                    for (int i = -blurRadius; i <= blurRadius; i++)
                    {
                        int idx = x + i;
                        if (idx < 0 || idx >= width)
                        {
                            idx = x; // Handle edge cases by using the current pixel
                        }

                        int pixel = originalBitmap.RawData[y * width + idx];
                        double weight = kernel[i + blurRadius];

                        a += ((pixel >> 24) & 0xFF) * weight; // Alpha
                        r += ((pixel >> 16) & 0xFF) * weight; // Red
                        g += ((pixel >> 8) & 0xFF) * weight;  // Green
                        b += (pixel & 0xFF) * weight;         // Blue
                    }

                    blurredData[y * width + x] = ((int)a << 24) | ((int)r << 16) | ((int)g << 8) | (int)b;
                }
            }

            // Apply the blur vertically
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double r = 0, g = 0, b = 0, a = 0;

                    for (int i = -blurRadius; i <= blurRadius; i++)
                    {
                        int idx = y + i;
                        if (idx < 0 || idx >= height)
                        {
                            idx = y; // Handle edge cases by using the current pixel
                        }

                        int pixel = blurredData[idx * width + x];
                        double weight = kernel[i + blurRadius];

                        a += ((pixel >> 24) & 0xFF) * weight; // Alpha
                        r += ((pixel >> 16) & 0xFF) * weight; // Red
                        g += ((pixel >> 8) & 0xFF) * weight;  // Green
                        b += (pixel & 0xFF) * weight;         // Blue
                    }

                    blurredBitmap.RawData[y * width + x] = ((int)a << 24) | ((int)r << 16) | ((int)g << 8) | (int)b;
                }
            }

            return blurredBitmap;
        }

        public static int[] ApplyAlpha(int[] data, float alphaFactor)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (Math.Max(0, Math.Min(255, (int)(((data[i] >> 24) & 0xFF) * alphaFactor))) << 24) | (((data[i] >> 16) & 0xFF) << 16) | (((data[i] >> 8) & 0xFF) << 8) | (data[i] & 0xFF);
            }
            return data;
        }

        public static Bitmap ApplyAlpha(Bitmap originalBitmap, float alphaFactor)
        {
            uint width = originalBitmap.Width;
            uint height = originalBitmap.Height;

            Bitmap resultBitmap = new Bitmap(width, height, originalBitmap.Depth);

            for (int i = 0; i < originalBitmap.RawData.Length; i++)
            {
                int pixel = originalBitmap.RawData[i];

                // Extract the alpha component and apply the alpha factor
                int alpha = Math.Max(0, Math.Min(255, (int)(((pixel >> 24) & 0xFF) * alphaFactor)));

                // Recombine the color components with the new alpha value
                resultBitmap.RawData[i] = (alpha << 24) | (pixel & 0x00FFFFFF);
            }

            return resultBitmap;
        }

    }
}
