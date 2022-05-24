namespace TestGame.UI.Rendering
{
    public interface IRenderable
    {
        Position Position { get; }

        int CurrentTextureWidth { get; }
        int CurrentTextureHeight { get; }

        Bitmap GetTexture();
    }
}
