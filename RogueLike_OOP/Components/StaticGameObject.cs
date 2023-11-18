using RogueLike.Components.Position;

namespace RogueLike.Components
{
    public class StaticGameObject : GameObject
    {
        public int Attack { get; set; }
        public StaticGameObject(Position2D pos,
                                int attack,
                                bool beingVisited = false,
                                bool visitable = false,
                                string defaultSymbol = " ")
                                :
                                base(pos, beingVisited, visitable, defaultSymbol) 
        {
            Attack = attack;
        }
    }
}