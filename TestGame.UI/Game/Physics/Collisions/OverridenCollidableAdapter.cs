namespace TestGame.UI.Game.Physics.Collisions;

public class OverridenCollidableAdapter : ICollidable
{
    public OverridenCollidableAdapter(ICollidable collidable, Position position)
    {
        Collidable = collidable;
        NewPosition = position;
        OldPosition = new Position(collidable.Hitbox.Location.X, collidable.Hitbox.Location.Y);
        NewHitbox = new RectangleF(position.X, position.Y, collidable.Hitbox.Width, collidable.Hitbox.Height);
    }

    public RectangleF Hitbox => Collidable.Hitbox;
    public RectangleF NewHitbox { get; }

    public ICollidable Collidable { get; }
    public Position OldPosition { get; }
    public Position NewPosition { get; }
}
