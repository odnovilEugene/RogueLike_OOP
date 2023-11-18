using System.Formats.Asn1;
using RogueLike.Components;
using RogueLike.Components.Position;
using RogueLike.Core;

namespace RogueLike
{
    public class Game
    {
        public int InitialDepth { get; set; }
        public int InitialWidth { get; set; }
        public int Seed { get; set; }
        public int Level { get; set; } = 1;
        public Map Map { get; set; }
        public Player Player { get; set; }
        public List<MovingGameObject> Enemies { get; set; }
        private List<StaticGameObject> StaticObjects { get; set; }
        private List<Projectile> Projectiles { get; set; }
        private Renderer Renderer { get; set; }
        private bool IsGameOver
        {
            get
            {
                return (Player.Hp <= 0) || (Enemies.Count == 0);
            }
        }
        
        public Game(int depth, int width, int seed)
        {
            InitialDepth = depth;
            InitialWidth = width;
            Seed = seed;
            Map = new Map(InitialDepth, InitialWidth, Seed);
            Player = new Player(new Position2D(1, 1));
            Enemies = GenerateEnemies(Map.Width / 3);
            StaticObjects = GenerateStaticObjects(Map.Width / 8);
            Projectiles = new List<Projectile>();
            Renderer = new Renderer();
        }

        private List<MovingGameObject> GenerateEnemies(int n)
        {
            List<MovingGameObject> enemies = new();

            int i = 0;
            int counter = 0;

            var field = Map.Field;
            while (i < n)
            {
                Random rand = new();
                Position2D enemyPos = new(rand.Next(3, Map.Depth - 1), rand.Next(3, Map.Width - 1));
                int y = enemyPos.Y;
                int x = enemyPos.X;
                if (field[y, x].Visitable && !field[y, x].BeingVisited)
                {
                    if (rand.Next(0, 2) == 0) 
                        enemies.Add(new Zombie(enemyPos));
                    else
                        enemies.Add(new Shooter(enemyPos));
                    field[y, x].BeingVisited = true;
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

        private List<StaticGameObject> GenerateStaticObjects(int n)
        {
            List<StaticGameObject> staticObjects = new();

            int i = 0;
            int counter = 0;

            var field = Map.Field;
            while (i < n)
            {
                Random rand = new();
                Position2D objPos = new(rand.Next(3, Map.Depth - 1), rand.Next(3, Map.Width - 1));
                int y = objPos.Y;
                int x = objPos.X;
                if (field[y, x].Visitable && !field[y, x].BeingVisited)
                {
                    staticObjects.Add(new AidKit(objPos));
                    field[y, x].BeingVisited = true;
                    i++;
                }
                if (counter > 100)
                {
                    Console.WriteLine("No place for static objects");
                    break;
                }
                counter++;
            }

            return staticObjects;
        }
       
        public void UpdateToNextLevel()
        {
            Level++;
            Player.Position = new Position2D(1, 1);
            Map = new Map(InitialDepth, InitialWidth, -1);
            Enemies = GenerateEnemies(Map.Width / 3);
            StaticObjects = GenerateStaticObjects(Map.Width / 8);
            Projectiles.Clear();
        }

        public void GameCycle()
        {   
            ConsoleKey key;
            do
            {
                key = StartGame();
                if (key == ConsoleKey.Enter)
                    break;
                Console.Write("Continue to next level?\ny/n : ");
                ConsoleKey answer = Console.ReadKey().Key;
                if (answer == ConsoleKey.Y)
                    UpdateToNextLevel();
                else if (answer == ConsoleKey.N)
                    break;
            } while (key != ConsoleKey.Enter);
        }

        public ConsoleKey StartGame()
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
            return key;
        }

        private static (int, int) KeyToDirection(ConsoleKey key)
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

        private static ConsoleKey DirectionToKey((int, int) direction)
        {
            return direction switch
            {
                (-1,0) => ConsoleKey.W,
                (0,-1) => ConsoleKey.A,
                (1, 0) => ConsoleKey.S,
                (0, 1) => ConsoleKey.D,
                _ => ConsoleKey.Enter
            };
        }

        private void MoveObject(MovingGameObject gameObject, ConsoleKey key)
        {
            (int dy, int dx) = KeyToDirection(key);
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
                    field[y + dy, x + dx].BeingVisited = true;
                    field[y, x].BeingVisited = false;
                    gameObject.Move(newPosition);
                    field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                } 
                else {
                    switch (gameObject)
                    {
                        case Player player:
                            var enemy = Enemies.Find(obj => obj.Position == newPosition);
                            if (enemy != null)
                            {
                                enemy.TakeDamage(gameObject.Attack);
                                if (enemy.IsDead)
                                {
                                    Enemies.Remove(enemy);
                                    field[y + dy, x + dx].BeingVisited = true;
                                    field[y, x].BeingVisited = false;
                                    gameObject.Move(newPosition);
                                    field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                                }
                            }
                            else
                            {
                                var projectile = Projectiles.Find(obj => obj.Position == newPosition);
                                if (projectile != null)
                                {
                                    Player.TakeDamage(projectile.Attack);
                                    Projectiles.Remove(projectile);
                                    field[y + dy, x + dx].BeingVisited = true;
                                    field[y, x].BeingVisited = false;
                                    gameObject.Move(newPosition);
                                    field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                                }
                                else 
                                {
                                    var staticObject = StaticObjects.Find(obj => obj.Position == newPosition);
                                    if (staticObject != null)
                                    {
                                        Player.TakeDamage(staticObject.Attack);
                                        StaticObjects.Remove(staticObject);
                                        field[y + dy, x + dx].BeingVisited = true;
                                        field[y, x].BeingVisited = false;
                                        gameObject.Move(newPosition);
                                        field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                                    }
                                }
                            }
                            break;
                        default:
                            if (Player.Position == newPosition)
                                Player.TakeDamage(gameObject.Attack);
                                if (gameObject is Projectile)
                                {
                                    field[y, x].BeingVisited = false;
                                    field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                                    Projectiles.Remove((Projectile)gameObject);
                                }
                            break;
                    }
                }
            }
            else
            {
                if (gameObject is Projectile projectile)
                {
                    field[y, x].BeingVisited = false;
                    field[y, x].CurrentSymbol = field[y, x].DefaultSymbol;
                    Projectiles.Remove(projectile);
                }
            }
        }

        private void EnemieMove(MovingGameObject enemy)
        {
            var field = Map.Field;
            int y = enemy.Position.Y;
            int x = enemy.Position.X;
            int player_y = Player.Position.Y;
            int player_x = Player.Position.X;
            static bool Passable(GameObject cell, bool isVerticalLine) 
            {
                if (isVerticalLine)
                {
                    return cell.Visitable && !cell.BeingVisited && (cell is not HorizontalWall);
                }
                else
                    return cell.Visitable && !cell.BeingVisited;
            }
            if (enemy.Position.Y == Player.Position.Y)
            {
                bool playerOnLeft = (x - player_x) > 0;
                bool spaceBetween = Math.Abs(x - player_x) != 1;

                int i = playerOnLeft ? player_x + 1 : x + 1;
                int end = playerOnLeft ? x : player_x;
                if (spaceBetween)
                {
                    for (; i < end; i++)
                    {
                        if (!Passable(Map.Field[y, i], false))
                        {
                            return;
                        }
                    }
                }
                ConsoleKey keyToMove = playerOnLeft ? ConsoleKey.A : ConsoleKey.D;
                switch (enemy)
                {
                    case Zombie zombie:
                        MoveObject(zombie, keyToMove);
                        break;
                    case Shooter shooter:
                        (int dy, int dx) = KeyToDirection(keyToMove);
                        if (!field[y + dy, x + dx].BeingVisited)
                            Projectiles.Add(shooter.Shoot((dy, dx)));
                        break;
                }

            }
            else
                if (enemy.Position.X == Player.Position.X)
                {
                    bool playerOnTop = (y - player_y) > 0;
                    bool spaceBetween = Math.Abs(y - player_y) != 1;

                    int i = playerOnTop ? player_y + 1 : y + 1;
                    int end = playerOnTop ? y : player_y;
                    if (Map.Field[i - 1, x] is HorizontalWall) return;
                    if (spaceBetween)
                    {
                        for (; i < end; i++)
                        {
                            if (!Passable(Map.Field[i, x], true))
                            {
                                return;
                            }
                        }
                    }
                    ConsoleKey keyToMove = playerOnTop ? ConsoleKey.W : ConsoleKey.S;
                    switch (enemy)
                    {
                        case Zombie zombie:
                            MoveObject(enemy, keyToMove);
                            break;
                        case Shooter shooter:
                            (int dy, int dx) = KeyToDirection(keyToMove);
                            if (!field[y + dy, x + dx].BeingVisited)
                                Projectiles.Add(shooter.Shoot((dy, dx)));
                            break;
                    }
                }
        }

        private void ProjectileMove(Projectile projectile)
        {
            MoveObject(projectile, DirectionToKey(projectile.Direction));
        }

        private void MakeTurn(ConsoleKey key)
        {   
            MoveObject(Player, key);
            foreach (var projectile in Projectiles.ToList())
            {
                ProjectileMove(projectile);
            }
            foreach (var enemy in Enemies.ToList())
            {
                EnemieMove(enemy);
            }
            PlaceEntities();
        }

        private void PlaceEntities()
        {
            PlacePlayer();
            PlaceEnemies();
            PlaceProjectiles();
            PlaceStaticObjects();
        }

        private void PlacePlayer()
        {
            var field = Map.Field;
            int y = Player.Position.Y;
            int x = Player.Position.X;
            field[y, x].BeingVisited = true;
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
                field[y, x].BeingVisited = true;
                if (field[y, x] is HorizontalWall)
                {
                    field[y, x].CurrentSymbol = $"{enemy.CurrentSymbol}\u0332".Normalize();
                } else field[y, x].CurrentSymbol = enemy.DefaultSymbol;
            }
        }
        
        private void PlaceProjectiles()
        {
            var field = Map.Field;
            foreach (var projectile in Projectiles)
            {
                int y = projectile.Position.Y;
                int x = projectile.Position.X;
                field[y, x].BeingVisited = true;
                if (field[y, x] is HorizontalWall)
                {
                    field[y, x].CurrentSymbol = $"{projectile.CurrentSymbol}\u0332".Normalize();
                } else field[y, x].CurrentSymbol = projectile.DefaultSymbol;
            }
        }
        
        private void PlaceStaticObjects()
        {
            var field = Map.Field;
            foreach (var obj in StaticObjects)
            {
                int y = obj.Position.Y;
                int x = obj.Position.X;
                field[y, x].BeingVisited = true;
                if (field[y, x] is HorizontalWall)
                {
                    field[y, x].CurrentSymbol = $"{obj.CurrentSymbol}\u0332".Normalize();
                } else field[y, x].CurrentSymbol = obj.DefaultSymbol;
            }
        }
        
        private bool InBounds(Position2D pos)
        {
            return 0 < pos.Y && pos.Y <= Map.Depth - 1 && 0 < pos.X && pos.X < Map.Width - 1;
        }
    }
}