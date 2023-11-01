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
    }
}