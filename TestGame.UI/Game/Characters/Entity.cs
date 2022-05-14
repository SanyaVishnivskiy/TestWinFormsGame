namespace TestGame.UI.Game.Characters;

public abstract class Entity : IRenderable
{
    public Entity(Position position, Animation animation)
    {
        Position = position;
        Animation = animation;
        Width = Animation.FirstFrame.Width;
        Height = Animation.FirstFrame.Height;
    }

    public Position Position { get; }
    public Animation Animation { get; }

    public virtual int Width { get; }
    public virtual int Height { get; }

    public Bitmap GetTexture()
    {
        return Animation.GetNextFrame();
    }
}
