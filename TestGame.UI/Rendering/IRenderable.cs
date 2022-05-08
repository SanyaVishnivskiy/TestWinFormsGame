namespace TestGame.UI.Rendering
{
    internal interface IRenderable
    {
        Position Position { get; }

        Bitmap GetTexture();
    }
}
