using RogueLike.Components;

namespace RogueLike.Core
{
    public class Renderer
    {
        public static void PrintGame(Game game)
        {
            var map = game.Map;
            var player = game.Player;
            var enemies = game.Enemies;
            Console.Clear();
            Console.WriteLine($"Level : {game.Level}");
            Console.WriteLine(map);
            PrintInfo(player, enemies);
        }
        public static void PrintInfo(Player player, List<MovingGameObject> enemies)
        {
            Console.WriteLine(player.GetInfo());
            foreach (var enemy in enemies)
            {
                Console.WriteLine(enemy.GetInfo());
            }
        }
    }
}