using RogueLike.Components;
using RogueLike.Components.Position;
using RogueLike.Core;

namespace RogueLike
{
    public class Game
    {
        public Map Map { get; set; }
        private Player Player { get; set; }
        private LivingGameObject[] Enemies { get; set; }
        
        public Game(int depth, int width, int seed)
        {
            Map = new Map(depth, width, seed);
            Player = new Player(new Position2D(1, 1));
            Enemies = GenerateEnemies(Map.Width / 3);
        }

        private LivingGameObject[] GenerateEnemies(int n)
        {
            LivingGameObject[] enemies = new LivingGameObject[n];

            int i = 0;
            int counter = 0;
            while (i < n)
            {
                Random rand = new();
                Position2D enemyPos = new(rand.Next(1, Map.Depth - 1), rand.Next(1, Map.Width - 1));
                int y = enemyPos.Y;
                int x = enemyPos.X;
                if ((Map.Field[y, x] is EmptyCell) || ((Map.Field[y, x] is HorizontalWall) && (Map.Field[y, x].CurrentSymbol == Map.Field[y, x].DefaultSymbol)))
                {
                    if (rand.Next(0, 2) == 0)
                        enemies[i] = new Zombie(new Position2D(enemyPos));
                    else
                        enemies[i] = new Shooter(new Position2D(enemyPos));
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
            PlaceEntities(Player, Enemies);
            Console.WriteLine(Map);
            PrintInfo(Player, Enemies);
            do
            {
                key = Console.ReadKey().Key;
                Player.Move(key, Map);
                PlaceEntities(Player, Enemies);
                Console.Clear();
                Console.WriteLine(Map);
                PrintInfo(Player, Enemies);
            } while (key != ConsoleKey.Enter);

        }

        private void PrintInfo(Player player, LivingGameObject[] enemies)
        {
            PrintPlayerInfo(player);
            PrintEnemiesInfo(enemies);
        }

        private void PrintPlayerInfo(Player player)
        {
            Console.WriteLine($"Player: Hp {player.Hp} / {player.MaxHp}, Position {player.Position} \n");
        }

        private void PrintEnemiesInfo(LivingGameObject[] enemies)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Console.WriteLine($"Enemy {i + 1}: Hp {enemies[i].Hp} / {enemies[i].MaxHp}, Position {enemies[i].Position}");
            }
        }

        private void PlaceEntities(Player player, LivingGameObject[] enemies)
        {
            PlacePlayer(player);
            PlaceEnemies(enemies);
        }

        private void PlacePlayer(Player player)
        {
            int y = player.Position.Y;
            int x = player.Position.X;
            Map.Field[y, x].BeingVisited = true;
            if (Map.Field[y, x] is HorizontalWall)
            {
                Map.Field[y, x].CurrentSymbol = $"{player.CurrentSymbol}\u0332".Normalize();
            } else Map.Field[y, x].CurrentSymbol = player.CurrentSymbol;
        }

        private void PlaceEnemies(LivingGameObject[] enemies)
        {
            foreach (LivingGameObject enemy in enemies)
            {
                int y = enemy.Position.Y;
                int x = enemy.Position.X;
                Map.Field[y, x].BeingVisited = true;
                if (Map.Field[y, x] is HorizontalWall)
                {
                    Map.Field[y, x].CurrentSymbol = $"{enemy.CurrentSymbol}\u0332".Normalize();
                } else Map.Field[y, x].CurrentSymbol = enemy.CurrentSymbol;
            }
        }
    }
}