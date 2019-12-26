using System;
using System.Drawing;
using System.IO;
using ImageMagick;
using JetBrains.Annotations;

namespace Aoc2019
{
    public static class MagickExtensions
    {
        public static byte[] ToPng<T>([NotNull] this T[,] @this, Func<T, Color> selector, int scaleFactor = 4, int border = 10)
        {
            var (cx, cy) = @this.GetDimensions();

            using var image = new MagickImage(MagickColors.Black, cx, cy);
            image.Density = new Density(100, DensityUnit.PixelsPerInch);

            using (var outPixels = image.GetPixels())
            {
                var bytes = new byte[3];
                foreach (var (x, y, c) in @this.SelectCells().SelectXy())
                {
                    var color = selector(c);
                    bytes[0] = color.R;
                    bytes[1] = color.G;
                    bytes[2] = color.B;
                    outPixels[x, y].Set(bytes);
                }
            }

            image.Scale(new Percentage(scaleFactor * 100));

            image.BorderColor = image.BackgroundColor;
            image.Border(border);

            var stream = new MemoryStream();
            image.Write(stream, MagickFormat.Png);

            return stream.GetBuffer();
        }
    }
}
