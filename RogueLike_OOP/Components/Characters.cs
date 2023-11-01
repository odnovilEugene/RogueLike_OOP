using RogueLike.Components.Position;

namespace RogueLike.Components
{
    public class Player : LivingGameObject
    {
        public Player(Position2D pos) : base("P", pos, 10, 2) {}
    }

    public class Zombie : LivingGameObject
    {
        public Zombie(Position2D pos) : base("Z", pos, 3, 1) {}
    }

    public class Shooter : LivingGameObject
    {
        public Shooter(Position2D pos) : base("S", pos, 1, 0) {}
    }
}