using RogueLike.Components.Position;

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

        public void Move(ConsoleKey direction, GameObject[,] field)
        {
            int y = Position.Y;
            int x = Position.X;
            switch (direction)
            {
                case ConsoleKey.W: case ConsoleKey.UpArrow:
                    if (field[y - 1, x].Visitable && field[y - 1, x] is not HorizontalWall)
                    {
                        Position = new Position2D(y - 1, x);
                        field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                    }
                    break;
                case ConsoleKey.D: case ConsoleKey.RightArrow:
                    if (field[y, x + 1].Visitable)
                    {
                        Position = new Position2D(y, x + 1);
                        field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                    }
                    break;
                case ConsoleKey.S: case ConsoleKey.DownArrow:
                    if (field[y + 1, x].Visitable && field[y, x] is not HorizontalWall)
                    {
                        Position = new Position2D(y + 1, x);
                        field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                    }
                    break;
                case ConsoleKey.A: case ConsoleKey.LeftArrow:
                    if (field[y, x - 1].Visitable)
                    {
                        Position = new Position2D(y, x - 1);
                        field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                    }
                    break;
            }
        }
        public void TakeDamage(int amount)
        {
            Hp -= amount;
        }
    }
}