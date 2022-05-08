namespace TestGame.UI.Extensions;

public static class TwoDimensionalArrayExtensions
{
    public static void Fill<T>(this T[,] array, T value)
    {
        for (int row = 0; row < array.GetLength(0); row++)
        {
            for (int column = 0; column < array.GetLength(1); column++)
            {
                array[row, column] = value;
            }
        }
    }

    public static IEnumerable<T> Traverse<T>(this T[,] array)
    {
        for (int row = 0; row < array.GetLength(0); row++)
        {
            for (int column = 0; column < array.GetLength(1); column++)
            {
                yield return array[row, column];
            }
        }
    }

    public static IEnumerable<T> TraverseWithFilter<T>(this T[,] array, Func<int, int, bool> filter)
    {
        for (int row = 0; row < array.GetLength(0); row++)
        {
            for (int column = 0; column < array.GetLength(1); column++)
            {
                if (filter(row, column))
                {
                    yield return array[row, column];
                }
            }
        }
    }

    public static void Loop<T>(this T[,] array, Action<int, int> action)
    {
        for (int row = 0; row < array.GetLength(0); row++)
        {
            for (int column = 0; column < array.GetLength(1); column++)
            {
                action(row, column);
            }
        }
    }

    private static void Loop(Action<int, int> action)
    {

    }
}
