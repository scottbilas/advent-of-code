namespace AoC
{
    public struct Point3F
    {
        public float X, Y, Z;
        public Point3F(float x, float y, float z)
            => (X, Y, Z) = (x, y, z);
    }
    public struct Point3
    {
        public int X, Y, Z;
        public Point3(int x, int y, int z)
            => (X, Y, Z) = (x, y, z);
    }

    // TODO: manhattan distance
}
