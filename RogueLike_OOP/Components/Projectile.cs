using RogueLike.Components.Position;

namespace RogueLike.Components
{
    public class Projectile : MovingGameObject
    {
        public (int, int) Direction { get; set; }

        public Projectile(Position2D pos,
                        (int, int) direction,
                        int attack,
                        int maxHp,
                        string defaultSymbol = "o")
                        :
                        base(pos, maxHp, attack, defaultSymbol)
        {
            Direction = direction;
        }

        // private string DirectionToSymbol()
        // {
        //     return Direction switch
        //     {
        //         (-1,0) => "A",
        //         (0,-1) => "A",
        //         (1, 0) => "A",
        //         (0, 1) => "A",
        //         _ => " "
        //     };
        // }
    }
}