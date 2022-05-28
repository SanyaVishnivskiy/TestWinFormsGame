namespace TestGame.UI.Game.Positioning;

public class Camera
{
    private readonly Entity _entity;

    public Camera(Entity entity, Size clientSize)
    {
        _entity = entity;
        Resize(clientSize);
    }

    public Position Position => _entity.Position;

    public Position CentralPosition => new Position(
        ClientSize.Width / 2 - Zoom(Position.X) - Zoom(_entity.CurrentTextureWidth) / 2,
        ClientSize.Height / 2 - Zoom(Position.Y) - Zoom(_entity.CurrentTextureHeight) / 2);

    public Size ClientSize { get; private set; }

    private int _resizeCoefficient;

    public void Resize(Size newClientSize)
    {
        ClientSize = newClientSize;
        _resizeCoefficient = (int)Math.Round((double)ClientSize.Width / Constants.DefaultWindowWidth);
    }

    public Position ToCameraPosition(Position position)
    {
        var center = CentralPosition;
        var x = Zoom(position.X) + center.X;
        var y = Zoom(position.Y) + center.Y;
        return new Position(x, y);
    }

    private int Zoom(float value) => CalculateResizeProportion(value);

    public Size CalculateTextureSize(Size entity)
    {
        return new Size(
            CalculateResizeProportion(entity.Width),
            CalculateResizeProportion(entity.Height));
    }

    private int CalculateResizeProportion(float value)
    {
        return (int)value * _resizeCoefficient;
    }
}
