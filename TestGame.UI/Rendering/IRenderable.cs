namespace TestGame.UI.Rendering
{
    public interface IRenderable
    {
        Bitmap CurrentTexture { get; }
        Position Position { get; }

        int Width { get; }
        int Height { get; }

        Bitmap GetTexture();
    }
}
