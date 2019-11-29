namespace AoC
{
    public static partial class Utils
    {
        public static void Swap<T>(ref T left, ref T right)
        {
            var temp = left;
            left = right;
            right = temp;
        }
    }
}
