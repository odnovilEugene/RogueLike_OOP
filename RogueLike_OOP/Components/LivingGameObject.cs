using RogueLike.Components.Position;
using RogueLike.Core;

namespace RogueLike.Components
{
    public abstract class LivingGameObject : GameObject
    {
        public int Hp { get; set; }
        public int Attack { get; set; }

        public LivingGameObject(string symbol, Position2D pos, int hp, int attack) : base(symbol, pos) 
        {
            Hp = hp;
            Attack = attack;
        }

        public void Move(ConsoleKey direction, Map map)
        {
            GameObject[,] field = map.Field;
            int y = Position.Y;
            int x = Position.X;
            int dy = 0;
            int dx = 0;
            bool canPass = false;
            switch (direction)
            {
                case ConsoleKey.W: case ConsoleKey.UpArrow:
                    dy = -1;
                    dx = 0;
                    if (field[y + dy, x + dx] is not HorizontalWall)
                        canPass = true;
                    break;
                case ConsoleKey.D: case ConsoleKey.RightArrow:
                    dy = 0;
                    dx = 1;
                    canPass = true;
                    break;
                case ConsoleKey.S: case ConsoleKey.DownArrow:
                    dy = 1;
                    dx = 0;
                    if (field[y, x] is not HorizontalWall)
                    {
                        canPass = true;
                    }
                    break;
                case ConsoleKey.A: case ConsoleKey.LeftArrow:
                    dy = 0;
                    dx = -1;
                    canPass = true;
                    break;
            }
            if (InBounds(new Position2D(y + dy, x + dx), map) && canPass && field[y + dy, x + dx].Visitable)
            {
                Position = new Position2D(y + dy, x + dx);
                field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
            }
        }
        public void TakeDamage(int amount)
        {
            Hp -= amount;
        }

        private static bool InBounds(Position2D pos, Map map)
        {
            return 0 < pos.Y && pos.Y <= map.Depth - 1 && 0 < pos.X && pos.X < map.Width - 1;
        }
    }
}