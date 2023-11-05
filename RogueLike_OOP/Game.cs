using System.Security.Cryptography.X509Certificates;
using RogueLike.Components;
using RogueLike.Components.Position;
using RogueLike.Core;

namespace RogueLike
{
    public class Game
    {
        public Map Map { get; set; }
        public Player Player { get; set; }
        public List<LivingGameObject> Enemies { get; set; }
        public Renderer Renderer { get; set; }
        private bool IsGameOver
        {
            get
            {
                return (Player.Hp <= 0) || (Enemies.Count == 0);
            }
        }
        
        public Game(int depth, int width, int seed)
        {
            Map = new Map(depth, width, seed);
            Player = new Player(new Position2D(1, 1));
            Enemies = GenerateEnemies(Map.Width / 3);
            Renderer = new Renderer();
        }

        private List<LivingGameObject> GenerateEnemies(int n)
        {
            List<LivingGameObject> enemies = new();

            int i = 0;
            int counter = 0;
            while (i < n)
            {
                Random rand = new();
                Position2D enemyPos = new(rand.Next(3, Map.Depth - 1), rand.Next(3, Map.Width - 1));
                int y = enemyPos.Y;
                int x = enemyPos.X;
                if (Map.Field[y, x].Visitable && !Map.Field[y, x].BeingVisited)
                {
                    if (rand.Next(0, 2) == 0)
                        enemies.Add(new Zombie(enemyPos));
                    else
                        enemies.Add(new Shooter(enemyPos));
                    i++;
                }
                if (counter > 100)
                {
                    Console.WriteLine("No place for enemies");
                    break;
                }
                counter++;
            }

            return enemies;
        }

        public void GameCycle()
        {
            ConsoleKey key;
            PlaceEntities();
            Renderer.PrintGame(this);
            do
            {
                key = Console.ReadKey().Key;
                MakeTurn(key);
                Renderer.PrintGame(this);

            } while ((key != ConsoleKey.Enter) && !IsGameOver);
            if (key != ConsoleKey.Enter) 
                Console.WriteLine("GAME OVER!");
        }

        private (int, int) Direction(ConsoleKey key)
        {
            return key switch
            {
                ConsoleKey.W => (-1,0),
                ConsoleKey.A => (0,-1),
                ConsoleKey.S => (1, 0),
                ConsoleKey.D => (0, 1),
                _ => (0,0)
            };
        }

        private void MoveObject(LivingGameObject gameObject, ConsoleKey key)
        {
            (int dy, int dx) = Direction(key);
            if (dy == 0 && dx == 0)
            {
                return;
            }
            int y = gameObject.Position.Y;
            int x = gameObject.Position.X;
            Position2D newPosition = new(y + dy, x + dx);
            var field = Map.Field;
            bool horizontalWallPassable = true;
            switch (dy)
            {
                case -1: 
                    if (field[y + dy, x + dx] is HorizontalWall)
                        horizontalWallPassable = false;
                    break;
                case 1:
                    if (field[y, x] is HorizontalWall)
                        horizontalWallPassable = false;
                    break;
            }
            //Visitable
            if (InBounds(newPosition) && horizontalWallPassable && field[y + dy, x + dx].Visitable)
            {
                //Empty
                if (!field[y + dy, x + dx].BeingVisited)
                {
                    gameObject.Position = newPosition;
                    field[y + dy, x + dx].BeingVisited = true;
                    field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                    field[y, x].BeingVisited = false;
                } else {
                    if ((gameObject is not Components.Player) && (Player.Position == (y + dy, x + dx)))
                    {
                        Player.TakeDamage(gameObject.Attack);
                    }
                    else
                    {
                        LivingGameObject? enemy = Enemies.Find(obj => obj.Position == (y + dy, x + dx));
                        if (enemy != null)
                        {
                            enemy.TakeDamage(gameObject.Attack);
                            if (enemy.IsDead)
                            {
                                Enemies.Remove(enemy);
                                gameObject.Position = newPosition;
                                field[y + dy, x + dx].BeingVisited = true;
                                field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                                field[y, x].BeingVisited = false;
                            }

                        }
                    }
                }
            }
        }

        private void AIMove(LivingGameObject enemy)
        {
            int y = enemy.Position.Y;
            int x = enemy.Position.X;
            int player_y = Player.Position.Y;
            int player_x = Player.Position.X;
            if (enemy.Position.Y == Player.Position.Y)
            {
                MoveObject(enemy, (x - player_x) > 0 ? ConsoleKey.A : ConsoleKey.D);
            }
            else
                if (enemy.Position.X == Player.Position.X)
                {
                    MoveObject(enemy, (y - player_y) > 0 ? ConsoleKey.W : ConsoleKey.S);
                }
        }

        private void MakeTurn(ConsoleKey key)
        {   
            MoveObject(Player, key);
            foreach (var enemy in Enemies)
            {
                AIMove(enemy);
            }
            PlaceEntities();
        }

        private void PlaceEntities()
        {
            PlacePlayer();
            PlaceEnemies();
        }

        private void PlacePlayer()
        {
            var field = Map.Field;
            int y = Player.Position.Y;
            int x = Player.Position.X;
            if (field[y, x] is HorizontalWall)
            {
                field[y, x].CurrentSymbol = $"{Player.CurrentSymbol}\u0332".Normalize();
            } else field[y, x].CurrentSymbol = Player.CurrentSymbol;
        }

        private void PlaceEnemies()
        {
            var field = Map.Field;
            foreach (var enemy in Enemies)
            {
                int y = enemy.Position.Y;
                int x = enemy.Position.X;
                if (field[y, x] is HorizontalWall)
                {
                    field[y, x].CurrentSymbol = $"{enemy.CurrentSymbol}\u0332".Normalize();
                } else field[y, x].CurrentSymbol = enemy.DefaultSymbol;
            }
        }

        private bool InBounds(Position2D pos)
        {
            return 0 < pos.Y && pos.Y <= Map.Depth - 1 && 0 < pos.X && pos.X < Map.Width - 1;
        }
    }
}