using RogueLike.Components.Position;

namespace RogueLike.Components
{
    public class Player : MovingGameObject
    {
        public Player(Position2D pos,
                    int maxHp = 10,
                    int attack = 2,
                    string defaultSymbol = "P")
                    :
                    base(pos, maxHp, attack, defaultSymbol) {}
    }

    public class Zombie : MovingGameObject
    {
        public Zombie(Position2D pos,
                    int maxHp = 3,
                    int attack = 1,
                    string defaultSymbol = "Z")
                    :
                    base(pos, maxHp, attack, defaultSymbol) {}
    }

    public class Shooter : MovingGameObject
    {
        public Shooter(Position2D pos,
                    int maxHp = 1,
                    int attack = 0,
                    string defaultSymbol = "S")
                    :
                    base(pos, maxHp, attack, defaultSymbol) {}

        public Projectile Shoot((int, int) direction)
        {
            (int dy, int dx) = direction;
            int y = Position.Y;
            int x = Position.X;
            Position2D pos = new Position2D(y + dy, x + dx);
            return new Projectile(pos, direction, 1, 1);
        }
    }
}