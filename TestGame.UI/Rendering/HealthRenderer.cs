namespace TestGame.UI.Rendering;

public class HealthRenderer
{
    private GameState _state;

    public HealthRenderer(GameState state)
    {
        _state = state;
    }

    public void Render(Graphics g, IRenderable renderable)
    {
        if (renderable is not Entity)
        {
            return;
        }

        var entity = (Entity)renderable;
        if (entity.Health.IsDead)
        {
            return;
        }

        var healthBarPosition = new Point((int)entity.Position.X, (int)entity.Position.Y - Health.HealthBarHeight - 1);
        var position = _state.Camera.ToCameraPosition(healthBarPosition);

        g.DrawImage(GetTexture(entity), position);
    }

    private Bitmap GetTexture(Entity entity)
    {
        var texture = entity.Health.GetTexture();
        var newSize = _state.Camera.CalculateTextureSize(new Size(texture.Width, texture.Height));
        return texture.Resize(newSize);
    }
}
