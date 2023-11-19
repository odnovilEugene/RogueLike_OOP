namespace RogueLike.Utils
{
    public class Utils
    {
        public static T[] Shuffle<T>(T[] array, Random rand)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rand.Next(n--);
                (array[k], array[n]) = (array[n], array[k]);
            }
            return array;
        }

        public static (int, int) KeyToDirection(ConsoleKey key)
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

        public static ConsoleKey DirectionToKey((int, int) direction)
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
    }
}