using System;
using System.Drawing;

namespace WindowsFormsClientSample
{
    public static class BitmapHelper
    {
        public static byte[,] GetGrayScaleBitmapPixels(string path)
        {
            var bmp = new Bitmap(path);
            byte[,] res = new byte[bmp.Width, bmp.Height];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    var pixel = bmp.GetPixel(i, j);
                    if (pixel.R != pixel.G || pixel.G != pixel.B || pixel.R != pixel.B)
                        throw new ArgumentException("Image must be in gray scale",nameof(path));
                    res[i, j] = pixel.R;
                }
            }

            return res;
        }
    }
}