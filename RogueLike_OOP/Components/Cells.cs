using RogueLike.Components.Position;

namespace RogueLike.Components
{
    public class VerticalWall : GameObject
    {
        public VerticalWall(Position2D pos,
                            bool beingVisited = false,
                            bool visitable = false,
                            string defaultSymbol = "|")
                            :
                            base(pos, beingVisited, visitable, defaultSymbol) {}
    }

    public class HorizontalWall : GameObject
    {
        public HorizontalWall(Position2D pos,
                            bool beingVisited = false,
                            bool visitable = true,
                            string defaultSymbol = "_")
                            :
                            base(pos, beingVisited, visitable, defaultSymbol) {}
    }

    public class EmptyCell : GameObject
    {
        public EmptyCell(Position2D pos,
                        bool beingVisited = false,
                        bool visitable = true,
                        string defaultSymbol = " ")
                        :
                        base(pos, beingVisited, visitable, defaultSymbol) {}
    }

    
}