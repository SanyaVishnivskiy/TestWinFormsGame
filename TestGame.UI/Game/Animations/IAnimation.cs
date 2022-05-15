
namespace TestGame.UI.Game.Animations
{
    public interface IAnimation
    {
        Bitmap CurrentFrame { get; }
        Rectangle FirstFrame { get; }
        Bitmap GetNextFrame();
    }

    public interface IDetailedAnimation : IAnimation
    {
        AnimationActionType Type { get; }
        int FrameCount { get; }
        TimeSpan FrameDelay { get; }
        Bitmap Image { get; }
        bool Loop { get; }
        int NextFrameOffset { get; }
    }
}