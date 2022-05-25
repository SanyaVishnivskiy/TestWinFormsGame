namespace TestGame.UI.Extensions;

public static class BitmapExtensions
{
    public static bool CheckSameAs(this Bitmap first, Bitmap second)
    {
        if (first is null && second is null)
        {
            return true;
        }
        
        if (first is null || second is null)
        {
            return false;
        }

        if (first.Width != second.Width || first.Height != second.Height)
        {
            return false;
        }

        for (int x = 0; x < first.Width; x++)
        {
            for (int y = 0; y < first.Height; y++)
            {
                if (first.GetPixel(x, y) != second.GetPixel(x, y))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
