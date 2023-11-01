using System.ComponentModel.Design;
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
            Enemies = GenerateEnemies(width / 3);
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
                if (Map.Field[enemyPos.Y, enemyPos.X] is EmptyCell)
                {
                    enemies[i] = new Zombie(new Position2D(enemyPos));
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
            PlaceEntitiesOnField(Player, Enemies);
            Console.WriteLine(Map);
            do
            {
                key = Console.ReadKey().Key;
                Player.Move(key, Map.Field);
                PlaceEntitiesOnField(Player, Enemies);
                Console.Clear();
                Console.WriteLine(Map);
            } while (key != ConsoleKey.Enter);

        }

        private void PlaceEntitiesOnField(Player player, LivingGameObject[] enemies)
        {
            PlacePlayer(player);
            PlaceEnemies(enemies);
        }

        private void PlacePlayer(Player player)
        {
            int y = player.Position.Y;
            int x = player.Position.X;
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
                if (Map.Field[y, x] is HorizontalWall)
                {
                    Map.Field[y, x].CurrentSymbol = $"{enemy.CurrentSymbol}\u0332".Normalize();
                } else Map.Field[y, x].CurrentSymbol = enemy.CurrentSymbol;
            }
        }
    }
}