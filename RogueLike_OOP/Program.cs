namespace RogueLike
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new(10, 10, -1);
            game.GameCycle();
        }
    }
}