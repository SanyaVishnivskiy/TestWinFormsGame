namespace TestGame.UI.Rendering;

public class Renderer
{
    private GameState _state;

    private IReadOnlyList<IRenderable> _entitiesToRender;
    private Dictionary<Bitmap, Bitmap> _bitmapsCache = new();

    private HealthRenderer _healthRenderer;

    public Renderer(GameState state)
    {
        _state = state;

        _healthRenderer = new(state);

        _state.OnAllGameEntitiesChange += OnEntitiesListChange;
        OnEntitiesListChange(this, new GameEntitiesChangeEventArgs(_state.AllGameEntities));
    }

    private void OnEntitiesListChange(object? sender, GameEntitiesChangeEventArgs e)
    {
        _entitiesToRender = e.Entities.OfType<IRenderable>().ToList();
    }

    public void Render(Graphics g)
    {
        foreach (var groundTile in _state.Map.GetGroundTiles())
        {
            DrawDependingOnCamera(g, groundTile);
        }

        foreach (var entity in _entitiesToRender)
        {
            DrawDependingOnCamera(g, entity);
            DrawEntityHealth(g, entity);
        }

        foreach (var entity in _state.AttackingEntities)
        {
            DrawDependingOnCamera(g, entity.Weapon);
        }

        foreach (var mapObject in _state.Map.GetMapObjects())
        {
            DrawDependingOnCamera(g, mapObject);
        }
    }

    private void DrawDependingOnCamera(Graphics g, IRenderable entity)
    {
        var position = _state.Camera.ToCameraPosition(entity.Position);
        var texture = GetTexture(entity);
        g.DrawImage(texture, position.X, position.Y);
    }

    private void DrawEntityHealth(Graphics g, IRenderable renderable)
    {
        _healthRenderer.Render(g, renderable);
    }

    private Bitmap GetTexture(IRenderable entity)
    {
        var texture = entity.GetTexture();

        if (_bitmapsCache.TryGetValue(texture, out var cachedTexture))
        {
            return cachedTexture;
        }

        var newSize = _state.Camera.CalculateTextureSize(new Size(entity.CurrentTextureWidth, entity.CurrentTextureHeight));
        var result = texture.Resize(newSize);
        _bitmapsCache.Add(texture, result);

        return result;
    }

    public void ClearBitmapCache()
    {
        _bitmapsCache.Clear();
    }
}
