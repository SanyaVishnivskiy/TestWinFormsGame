namespace TestGame.UI.Rendering
{
    public interface IRenderable
    {
        Position Position { get; }

        int Width { get; }
        int Height { get; }

        Bitmap GetTexture();
    }
}
