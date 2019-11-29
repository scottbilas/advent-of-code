<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Drawing.Drawing2D</Namespace>
</Query>

var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

// sample

var sample = Solve($"{scriptDir}/sample.txt", 10);
sample.letters.ShouldBe("HI");
sample.time.ShouldBe(3);

// problem

var problem = Solve($"{scriptDir}/input.txt", 15);
problem.letters.Dump().ShouldBe("RGRKHKNA");
problem.time.Dump().ShouldBe(10117);

(string letters, int time) Solve(string path, int windowHeight)
{
    var positions = new List<(int x, int y)>();
    var velocities = new List<(int x, int y)>();

    File
        .ReadLines(path)
        .Select(line => Regex
            .Matches(line, @"-?\d+").Cast<Match>()
            .Select(m => int.Parse(m.Value))
            .ToList())
        .ForEach(ints =>
        {
            positions.Add((ints[0], ints[1]));
            velocities.Add((ints[2], ints[3]));
        });

    for (var time = 0;; ++time)
    {
        var bounds = (
            l: positions.Min(v => v.x), r: positions.Max(v => v.x) + 1,
            t: positions.Min(v => v.y), b: positions.Max(v => v.y) + 1);
        var dims = (w: bounds.r - bounds.l, h: bounds.b - bounds.t);
        if (dims.h < windowHeight)
        {
            // to detect text this small, tesseract needs black on white with a border and scaled up 400%
            
            var renderPath = path.Replace(".txt", ".png");
            var border = 10;
            using (var image = new Bitmap(dims.w + border * 2, dims.h + border * 2, PixelFormat.Format24bppRgb))
            {
                using (var gr = Graphics.FromImage(image))
                {
                    gr.Clear(Color.White);
                    foreach (var p in positions)
                        image.SetPixel(p.x - bounds.l + border, p.y - bounds.t + border, Color.Black);
                }

                var scaled = new Bitmap(image.Width * 4, image.Height * 4);
                using (var gr = Graphics.FromImage(scaled))
                    gr.DrawImage(image, 0, 0, scaled.Width, scaled.Height);
                scaled.Save(renderPath, ImageFormat.Png);
            }

            var ocrName = Path.GetFileNameWithoutExtension(path) + "_ocr";
            using (var process = Process.Start(new ProcessStartInfo
            {
                FileName = "tesseract",
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                Arguments = $"{renderPath} {ocrName}",
                WorkingDirectory = scriptDir,
            }))
            {
                process.WaitForExit();
            }

            var letters = File.ReadAllText($"{scriptDir}/{ocrName}.txt").Trim();

            return (letters, time);
        }

        for (int i = 0; i < positions.Count; ++i)
        {
            var pos = positions[i];
            var vel = velocities[i];
            positions[i] = (pos.x + vel.x, pos.y + vel.y);
        }
    }
}
