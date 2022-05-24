namespace TestGame.UI.Extensions;

internal static class AnimationsExtensions
{
    public static int CalculateFrameWidth(this IAnimation animation, int initialWidth)
        => CalculateCurrentFrameLength(initialWidth, animation.CurrentFrame.Width, animation.FirstFrame.Width);

    public static int CalculateFrameHeight(this IAnimation animation, int initialHeight)
        => CalculateCurrentFrameLength(initialHeight, animation.CurrentFrame.Height, animation.FirstFrame.Height);

    private static int CalculateCurrentFrameLength(int initialLength, int currentFrameLength, int firstFrameLength)
        => initialLength * currentFrameLength / firstFrameLength;
}
