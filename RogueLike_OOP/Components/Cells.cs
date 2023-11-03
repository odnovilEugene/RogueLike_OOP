using RogueLike.Components.Position;

namespace RogueLike.Components
{
    public class VerticalWall : GameObject
    {
        public VerticalWall(Position2D pos) : base(pos, false, false, "|") {}
    }

    public class HorizontalWall : GameObject
    {
        public HorizontalWall(Position2D pos, bool beingVisited = false) : base(pos, beingVisited, true, "_") {}
    }

    public class EmptyCell : GameObject
    {
        public EmptyCell(Position2D pos, bool beingVisited = false) : base(pos, beingVisited, true, " ") {}
    }
}