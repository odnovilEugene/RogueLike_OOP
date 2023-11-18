using RogueLike.Components.Position;

namespace RogueLike.Components
{
    public class AidKit : StaticGameObject
    {
        public AidKit(Position2D pos,
                    int attack = -5,
                    bool beingVisited = false,
                    bool visitable = false,
                    string defaultSymbol = "+")
                    :
                    base(pos, attack, beingVisited, visitable, defaultSymbol) {}
    }
}