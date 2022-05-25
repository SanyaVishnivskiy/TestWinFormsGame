namespace TestGame.UI.Game.Animations;

public class AnimationFrames
{
    private static readonly Dictionary<AnimationFrameKey, Bitmap> _bitmaps = new();
    //private static int i = 0;

    public static Bitmap GetOrCreate(Bitmap image, Rectangle frame, bool flipHorizontally, bool flipVerticallly, bool rotateLeft)
    {
        var key = new AnimationFrameKey(image, frame, flipHorizontally, flipVerticallly, rotateLeft);
        if (_bitmaps.TryGetValue(key, out var value))
        {
            return value;
        }

        var bitmap = CropFrame(key);
        _bitmaps.Add(key, bitmap);
        //i++;
        //Logger.Log("Total unique bitmaps " + i);
        return bitmap;
    }

    private static Bitmap CropFrame(AnimationFrameKey key)
    {
        var frame = new Bitmap(key.Frame.Width, key.Frame.Height);
        using var graphics = Graphics.FromImage(frame);

        var cropRect = new Rectangle(key.Frame.X, key.Frame.Y, frame.Width, frame.Height);

        graphics.DrawImage(key.Image, new Rectangle(0, 0, frame.Width, frame.Height), cropRect, GraphicsUnit.Pixel);

        if (key.RotateLeft)
        {
            frame.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }

        if (key.FlipVertically)
        {
            frame.RotateFlip(RotateFlipType.Rotate180FlipNone);
        }

        if (key.FlipHorizontally)
        {
            frame.RotateFlip(RotateFlipType.RotateNoneFlipX);
        }

        return frame;
    }
}

public class AnimationFrameKey
{
    public Bitmap Image { get; set; }
    public Rectangle Frame { get; set; }
    public bool FlipVertically { get; }
    public bool FlipHorizontally { get; }
    public bool RotateLeft { get; }

    public AnimationFrameKey(Bitmap image, Rectangle frame, bool flipHorizontally, bool flipVertically, bool rotateLeft)
    {
        Image = image;
        Frame = frame;
        FlipVertically = flipVertically;
        FlipHorizontally = flipHorizontally;
        RotateLeft = rotateLeft;
    }

    public override bool Equals(object? obj)
    {
        if (this is null && obj is null)
        {
            return true;
        }

        if (this is null || obj is null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        var other = (AnimationFrameKey)obj;

        return Image.CheckSameAs(other.Image) 
            && Frame.Equals(other.Frame)
            && FlipVertically == other.FlipVertically
            && FlipHorizontally == other.FlipHorizontally
            && RotateLeft == other.RotateLeft;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Image, Frame, FlipVertically, FlipHorizontally, RotateLeft);
    }
}
