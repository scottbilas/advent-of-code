<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
</Query>

// i was hoping the fabric when rendered would reveal an image used in a later puzzle, but there's no obvious shapes there

var scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

var rects = File
    .ReadLines($"{scriptDir}/input.txt")
    .Select(textRect => Regex
        .Matches(textRect, @"\d+").Cast<Match>()
        .Select(m => int.Parse(m.Value))
       .ToList())
    .Select(ints => (l: ints[1], t: ints[2], w: ints[3], h: ints[4]));

var fabric = new int[1000, 1000];
var maxCount = 0;

foreach (var rect in rects)
    for (int x = rect.l; x < rect.l + rect.w; ++x)
        for (int y = rect.t; y < rect.t + rect.h; ++y)
            maxCount = Math.Max(maxCount, ++fabric[x, y]);

using (var b = new Bitmap(1000, 1000))
{
    for (int y = 0; y < 1000; ++y)
    {
        for (int x = 0; x < 1000; ++x)
        {
            var c = fabric[x, y] * (255 / maxCount);
            b.SetPixel(x, y, Color.FromArgb(255, c, c, c));
        }
    }
    
    b.Save($"{scriptDir}/fabric.png", ImageFormat.Png);
}