using RogueLike.Components.Position;
using RogueLike.Core;

namespace RogueLike.Components
{
    public abstract class LivingGameObject : GameObject
    {
        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }

        public LivingGameObject(Position2D pos, int maxHp, int attack, string defaultSymbol) : base(pos, false, false, defaultSymbol) 
        {
            MaxHp = maxHp;
            Hp = MaxHp;
            Attack = attack;
        }

        public void Move(ConsoleKey direction, Map map)
        {
            GameObject[,] field = map.Field;
            int y = Position.Y;
            int x = Position.X;
            int dy = 0;
            int dx = 0;
            bool horizontalWallPassable = true;
            switch (direction)
            {
                case ConsoleKey.W: case ConsoleKey.UpArrow:
                    dy = -1;
                    dx = 0;
                    if (field[y + dy, x + dx] is HorizontalWall)
                        horizontalWallPassable = false;
                    break;
                case ConsoleKey.D: case ConsoleKey.RightArrow:
                    dy = 0;
                    dx = 1;
                    break;
                case ConsoleKey.S: case ConsoleKey.DownArrow:
                    dy = 1;
                    dx = 0;
                    if (field[y, x] is HorizontalWall)
                    {
                        horizontalWallPassable = false;
                    }
                    break;
                case ConsoleKey.A: case ConsoleKey.LeftArrow:
                    dy = 0;
                    dx = -1;
                    break;
            }
            if (InBounds(new Position2D(y + dy, x + dx), map) && horizontalWallPassable && field[y + dy, x + dx].Visitable && !field[y + dy, x + dx].BeingVisited)
            {
                Position = new Position2D(y + dy, x + dx);
                field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                field[y, x].BeingVisited = false;
            }
        }
        // public void TakeDamage(int amount, LivingGameObject[] enemies)
        // {
        //     Hp -= amount;
        //     if (Hp <= 0)
        //     {
        //         enemies = enemies.Where(value => value != this).ToArray();
        //         return enemies;
        //     }
        //     return enemies;
        // }

        private static bool InBounds(Position2D pos, Map map)
        {
            return 0 < pos.Y && pos.Y <= map.Depth - 1 && 0 < pos.X && pos.X < map.Width - 1;
        }
    }
}