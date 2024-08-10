using Cosmos.System.Graphics;
using ScrewOS.gui;

namespace Cobalt.GetIMG
{
	public static class TakeBitmap
	{
		/// <summary>
		/// With this function you can cache pixels from your canvas!
		/// </summary>
		public static Bitmap GetImage(int X, int Y, int Width, int Height)
		{
			Bitmap bitmap = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);

			for (int y = Y, destY = 0; y < Y + Height; y++, destY++)
				for (int x = X, destX = 0; x < X + Width; x++, destX++)
					bitmap.RawData[destY * Width + destX] = GuiHost.canvas.GetPointColor(x, y).ToArgb();

			return bitmap;
		}
	}
}
