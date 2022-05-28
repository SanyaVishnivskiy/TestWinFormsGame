using System.Drawing.Drawing2D;

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

    public static Bitmap Resize(this Bitmap bitmap, Size newSize)
    {
        if (bitmap.IsOneColored())
        {
            return bitmap.ResizeOneColored(newSize);
        }

        if (HasIntProportion(bitmap, newSize))
        {
            return bitmap.ResizeWithIntProportion(newSize);
        }

        Bitmap newBitmap = new Bitmap(newSize.Width, newSize.Height);
        using (Graphics graphics = Graphics.FromImage(newBitmap))
        {
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(bitmap, 0, 0, newSize.Width, newSize.Height);
        }

        return newBitmap;
    }

    private static bool HasIntProportion(Bitmap bitmap, Size newSize)
    {
        return (newSize.Width % bitmap.Width == 0) && (newSize.Height % bitmap.Height == 0);
    }

    public static Bitmap ResizeWithIntProportion(this Bitmap bitmap, Size newSize)
    {
        var widthProportion = newSize.Width / bitmap.Width;
        var heightProportion = newSize.Height / bitmap.Height;

        Bitmap newBitmap = new Bitmap(newSize.Width, newSize.Height);
        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                var pixel = bitmap.GetPixel(x, y);
                for (int i = 0; i < widthProportion; i++)
                {
                    for (int j = 0; j < heightProportion; j++)
                    {
                        newBitmap.SetPixel(x * widthProportion + i, y * heightProportion + j, pixel);
                    }
                }
            }
        }
        return newBitmap;
    }

    public static bool IsOneColored(this Bitmap bitmap)
    {
        var color = bitmap.GetPixel(0, 0);
        for (int x = 0; x < bitmap.Width; x++)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                if (bitmap.GetPixel(x, y) != color)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static Bitmap ResizeOneColored(this Bitmap bitmap, Size newSize)
    {
        var color = bitmap.GetPixel(0, 0);
        var newBitmap = new Bitmap(newSize.Width, newSize.Height);
        for (int x = 0; x < newSize.Width; x++)
        {
            for (int y = 0; y < newSize.Height; y++)
            {
                newBitmap.SetPixel(x, y, color);
            }
        }

        return newBitmap;
    }
}
