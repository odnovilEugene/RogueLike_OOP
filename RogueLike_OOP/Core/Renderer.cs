using RogueLike.Components;

namespace RogueLike.Core
{
    public class Renderer
    {
        public void PrintGame(Game game)
        {
            var map = game.Map;
            var player = game.Player;
            var enemies = game.Enemies;
            Console.Clear();
            Console.WriteLine(map);
            PrintInfo(player, enemies);
        }
        public void PrintInfo(Player player, List<LivingGameObject> enemies)
        {
            Console.WriteLine(player.GetInfo());
            foreach (var enemy in enemies)
            {
                Console.WriteLine(enemy.GetInfo());
            }
        }
    }
}