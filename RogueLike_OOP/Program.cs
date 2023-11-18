using RogueLike.Components;

namespace RogueLike
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new(5, 5, 124561724);
            game.GameCycle();
        }
    }
}