namespace TestGame.UI.Rendering
{
    public interface IRenderable
    {
        Position Position { get; }

        Bitmap GetTexture();
    }
}
