namespace RogueLike.Components.Position
{
    public struct Position2D
    {
        public int Y { get; set; }
        public int X { get; set; }

        public Position2D(int y, int x)
        {
            Y = y;
            X = x;
        }

        public Position2D(Position2D pos)
        {
            Y = pos.Y;
            X = pos.X;
        }
        public override string ToString()
        {
            return $"({Y}, {X})";
        }
    }


}