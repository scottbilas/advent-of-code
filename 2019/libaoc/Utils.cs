using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ImageMagick;
using Unity.Coding.Utils;

namespace Aoc2019
{
    public static partial class Utils
    {
        public static void Swap<T>(ref T left, ref T right)
        {
            var temp = left;
            left = right;
            right = temp;
        }

        static byte[] k_On = { 0, 0, 0 };

        public static void RenderForOcr(NPath path, bool[,] pixels)
        {
            // to detect small text, tesseract needs black on white with a border and scaled up 400%
            using (var image = new MagickImage(MagickColors.White, pixels.GetLength(0), pixels.GetLength(1)))
            {
                using (var outPixels = image.GetPixels())
                {
                    for (var y = 0; y < image.Height; ++y)
                        for (var x = 0; x < image.Width; ++x)
                            if (pixels[x, y])
                                outPixels[x, y].Set(k_On);
                }

                image.Scale(new Percentage(400));

                image.BorderColor = image.BackgroundColor;
                image.Border(10);

                image.Write(path, MagickFormat.Png);
            }
        }

        public static IEnumerable<string> RenderToText(bool[,] pixels)
        {
            var size = pixels.GetDimensions();
            
            for (var y = 0; y < size.Height; ++y)
                yield return new string(Enumerable.Range(0, size.Width).Select(x => pixels[x, y] ? 'X' : ' ').ToArray());
        }

        public static string Ocr(NPath imagePath)
        {
            var ocrBasePath = imagePath.Parent.Combine(imagePath.FileNameWithoutExtension + "-ocr");
            var ocrPath = ocrBasePath.Parent.Combine(ocrBasePath.FileName + ".txt");

            try
            {
                using (var process = Process.Start(new ProcessStartInfo
                {

                    FileName         = "tesseract", // scoop install tesseract
                    WindowStyle      = ProcessWindowStyle.Hidden,
                    Arguments        = $"{imagePath} {ocrBasePath}",
                    WorkingDirectory = imagePath.Parent,
                }))
                {
                    process.WaitForExit();
                    return ocrPath.ReadAllText().Trim();
                }
            }
            finally
            {
                ocrPath.DeleteIfExists();
            }
        }
    }
}
