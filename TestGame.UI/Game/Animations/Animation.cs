namespace TestGame.UI.Game.Animations;

public class Animation : IDetailedAnimation
{
    private readonly List<Bitmap> frames = new();

    private int _currentFrame = 0;
    private bool _playedOnce = false;
    private DateTime _lastFrameUpdated = DateTime.MinValue;

    public Animation(
        Bitmap image,
        Rectangle firstFrame,
        int frameCount,
        int nextFrameOffset,
        TimeSpan frameDelay,
        bool loop,
        AnimationActionType type,
        bool flipVertically,
        bool flipHorizontally,
        bool rotateLeft)
    {
        Image = image;
        FirstFrame = firstFrame;
        FrameCount = frameCount;
        NextFrameOffset = nextFrameOffset;
        FrameDelay = frameDelay;
        Loop = loop;
        Type = type;
        FlipVertically = flipVertically;
        FlipHorizontally = flipHorizontally;
        RotateLeft = rotateLeft;

        InitFrames();
    }

    public Bitmap Image { get; }
    public Rectangle FirstFrame { get; }
    public int FrameCount { get; }
    public int NextFrameOffset { get; }
    public TimeSpan FrameDelay { get; }
    public bool Loop { get; }
    public AnimationActionType Type { get; }
    public bool FlipVertically { get; }
    public bool FlipHorizontally { get; }
    public bool RotateLeft { get; }

    public Bitmap CurrentFrame => frames[_currentFrame];

    private void InitFrames()
    {
        for (int i = 0; i < FrameCount; i++)
        {
            frames.Add(GetFrame(i));
        }
    }

    private Bitmap GetFrame(int i)
    {
        var offset = i > 0 ? NextFrameOffset : 0;
        var frameX = FirstFrame.X + ((FirstFrame.Width + offset) * i);
        var frame = new Rectangle(frameX, FirstFrame.Y, FirstFrame.Width, FirstFrame.Height);
        return AnimationFrames.GetOrCreate(Image, frame, FlipHorizontally, FlipVertically, RotateLeft);
    }

    public Bitmap GetNextFrame()
    {
        if (_playedOnce && !Loop)
        {
            return frames[0];
        }

        if (DateTime.Now - _lastFrameUpdated > FrameDelay)
        {
            _currentFrame++;
            _lastFrameUpdated = DateTime.Now;
        }

        if (_currentFrame >= FrameCount)
        {
            _currentFrame = 0;
            _playedOnce = true;
        }

        return CurrentFrame;
    }

    public void Reset()
    {
        _currentFrame = 0;
        _playedOnce = false;
    }

    public static IAnimationsBuilder New() => new AnimationsBuilder();
    public static IAnimationAggregatesBuilder NewAggregate() => new AnimationAggregatesBuilder();
}
