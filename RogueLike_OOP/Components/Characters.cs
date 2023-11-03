using RogueLike.Components.Position;

namespace RogueLike.Components
{
    public class Player : LivingGameObject
    {
        public Player(Position2D pos) : base(pos, 10, 2, "P") {}
    }

    public class Zombie : LivingGameObject
    {
        public Zombie(Position2D pos) : base(pos, 3, 1, "Z") {}
    }

    public class Shooter : LivingGameObject
    {
        public Shooter(Position2D pos) : base(pos, 1, 0, "S") {}
    }
}