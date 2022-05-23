namespace TestGame.UI.Game.Animations;

public interface IAnimationsBuilder
{
    Animation Build();
    IAnimationsBuilder FromSprite(Bitmap sprites);
    IAnimationsBuilder Looped();
    IAnimationsBuilder WithFirstFrame(Rectangle firstFrame);
    IAnimationsBuilder WithFrameCount(int frameCount);
    IAnimationsBuilder WithFrameDelay(TimeSpan frameDelay);
    IAnimationsBuilder WithFrameOffset(int frameOffsetInPixels);
    IAnimationsBuilder FlipVertically();
    IAnimationsBuilder FlipHorizontally();
    IAnimationsBuilder RotateLeft();
}

public interface IAggregatedAnimationBuilder : IAnimationsBuilder
{
    IAnimationsBuilder OfType(AnimationActionType type);
}

public class AnimationsBuilder : IAggregatedAnimationBuilder
{
    private Bitmap? _sprites;
    private Rectangle _firstFrame = new Rectangle(0, 0, Constants.TileWidth, Constants.TileHeight);
    private int _frameCount = 1;
    private int _frameOffset;
    private TimeSpan _frameDelay = TimeSpan.FromMilliseconds(500);
    private bool _loop;
    private AnimationActionType _type;
    private bool _flipVertically;
    private bool _flipHorizontally;
    private bool _rotateLeft;

    public IAnimationsBuilder FromSprite(Bitmap sprites)
    {
        _sprites = sprites;
        return this;
    }

    public IAnimationsBuilder WithFirstFrame(Rectangle firstFrame)
    {
        _firstFrame = firstFrame;
        return this;
    }

    public IAnimationsBuilder WithFrameCount(int frameCount)
    {
        _frameCount = frameCount;
        return this;
    }

    public IAnimationsBuilder WithFrameOffset(int frameOffsetInPixels)
    {
        _frameOffset = frameOffsetInPixels;
        return this;
    }

    public IAnimationsBuilder WithFrameDelay(TimeSpan frameDelay)
    {
        _frameDelay = frameDelay;
        return this;
    }

    public IAnimationsBuilder Looped()
    {
        _loop = true;
        return this;
    }

    public IAnimationsBuilder OfType(AnimationActionType type)
    {
        _type = type;
        return this;
    }

    public IAnimationsBuilder FlipVertically()
    {
        _flipVertically = true;
        return this;
    }

    public IAnimationsBuilder FlipHorizontally()
    {
        _flipHorizontally = true;
        return this;
    }

    public IAnimationsBuilder RotateLeft()
    {
        _rotateLeft = true;
        return this;
    }

    public Animation Build()
    {
        if (_sprites == null)
            throw new ArgumentNullException(nameof(_sprites));
        if (_firstFrame.Width == 0 || _firstFrame.Height == 0)
            throw new ArgumentException(nameof(_firstFrame));

        return new Animation(_sprites, _firstFrame, _frameCount, _frameOffset, _frameDelay, _loop, _type, _flipVertically, _flipHorizontally, _rotateLeft);
    }
}
