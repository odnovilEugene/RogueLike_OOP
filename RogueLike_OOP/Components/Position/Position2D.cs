using System.Runtime.CompilerServices;

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
        
        public static bool operator ==(Position2D a, Position2D b)
        {
            return (a.Y == b.Y) && (a.X == b.X);
        }

        public static bool operator !=(Position2D a, Position2D b)
        {
            return (a.Y != b.Y) || (a.X != b.X);
        }

        public static bool operator ==(Position2D a, (int, int) coords)
        {
            return (coords.Item1 == a.Y) && (coords.Item2 == a.X);
        }

        public static bool operator !=(Position2D a, (int, int) coords)
        {
            return (coords.Item1 != a.Y) || (coords.Item2 != a.X);
        }

        public override string ToString()
        {
            return $"({Y}, {X})";
        }
    }
}