using System;
using Unity.Coding.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageMagick;
using Unity.Coding.Editor;
using static Aoc2017.MiscStatics;

namespace Aoc2017
{
    public sealed class Void { }

    public static partial class Utils
    {
        public static void Swap<T>(ref T left, ref T right)
        {
            var temp = left;
            left = right;
            right = temp;
        }

        static byte[] k_On = { 0, 0, 0 };

        public static string OcrSmallText(Int2 size, Func<Int2, bool> isSet)
        {
            var imagePath = Path.GetTempFileName().ToNPath();
            try
            {
                // to detect small text, tesseract needs black on white with a border and scaled up 400%
                using (var image = new MagickImage(MagickColors.White, size.X, size.Y))
                {
                    image.Density = new Density(100, DensityUnit.PixelsPerInch);

                    using (var outPixels = image.GetPixels())
                    {
                        for (var y = 0; y < image.Height; ++y)
                            for (var x = 0; x < image.Width; ++x)
                                if (isSet(new Int2(x, y)))
                                    outPixels[x, y].Set(k_On);
                    }

                    image.Scale(new Percentage(400));

                    image.BorderColor = image.BackgroundColor;
                    image.Border(10);

                    image.Write(imagePath, MagickFormat.Png);
                }

                var (stdout, stderr) = (new List<string>(), new List<string>());
                ProcessUtility.ExecuteCommandLine("tesseract", Arr(imagePath.ToString(), "stdout"), null, stdout, stderr);
                return stdout[0];
            }
            finally
            {
                imagePath.DeleteIfExists();
            }
        }

        public static string OcrSmallText(bool[,] pixels) =>
            OcrSmallText(pixels.GetDimensions(), pos => pixels[pos.X, pos.Y]);

        public static void RenderGraphViz(NPath png, string dot, params string[] extraArgs)
        {
            var dotPath = NPath.SystemTemp.Combine(Guid.NewGuid() + ".dot");
            var renderPath = dotPath.ChangeExtension(".png");

            try
            {
                dotPath.WriteAllText(dot);
                ProcessUtility.ExecuteCommandLine("dot", $"-Gdpi=175 -Nfontname=Roboto -Efontname=Roboto -Tpng {dotPath} -o {renderPath}".Split(' ').Concat(extraArgs), null, null, null);
                ProcessUtility.ExecuteCommandLine("magick", $"{renderPath} -channel RGB -negate {png}".Split(' '), null, null, null);
            }
            finally
            {
                dotPath.DeleteIfExists();
                renderPath.DeleteIfExists();
            }
        }

        public static byte[] RenderGraphViz(string dot, params string[] extraArgs)
        {
            var pngPath = NPath.SystemTemp.Combine(Guid.NewGuid() + ".png");

            try
            {
                RenderGraphViz(pngPath, dot, extraArgs);
                return pngPath.ReadAllBytes();
            }
            finally
            {
                pngPath.DeleteIfExists();
            }
        }

        public static int Gcd(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            for (;;)
            {
                var r = a % b;
                if (r == 0)
                    return b;
                a = b;
                b = r;
            };
        }

        public static long Gcd(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            for (;;)
            {
                var r = a % b;
                if (r == 0)
                    return b;
                a = b;
                b = r;
            };
        }

        public static int Gcd(IEnumerable<int> nums) => nums.Aggregate(Gcd);
        public static int Gcd(params int[] nums) => Gcd(nums.AsEnumerable());
        public static long Gcd(IEnumerable<long> nums) => nums.Aggregate(Gcd);
        public static long Gcd(params long[] nums) => Gcd(nums.AsEnumerable());

        public static int Lcm(params int[] nums) => Lcm(nums.AsEnumerable());
        public static long Lcm(params long[] nums) => Lcm(nums.AsEnumerable());

        public static int Lcm(int a, int b)
        {
            checked { return a * b / Gcd(a, b); }
        }

        public static long Lcm(long a, long b)
        {
            checked { return a / Gcd(a, b) * b; }
        }

        public static int Lcm(IEnumerable<int> nums)
        {
            checked { return nums.Aggregate((a, b) => a / Gcd(a, b) * b); }
        }

        public static long Lcm(IEnumerable<long> nums)
        {
            checked { return nums.Aggregate((a, b) => a / Gcd(a, b) * b); }
        }

        public static IEnumerable<int> Factors(int num)
        {
            for (var i = (int)Math.Sqrt(num) + 1; i >= 1; --i)
                if (num % i == 0)
                    yield return i;
        }

        public static void Minimize(ref int a, int b) => a = Math.Min(a, b);
        public static void Maximize(ref int a, int b) => a = Math.Max(a, b);
    }
}
