namespace TestGame.UI.Game.Physics.Collisions;

public class OverridenCollidableAdapter : ICollisionTrackable
{
    public OverridenCollidableAdapter(ICollisionTrackable collidable, Move move)
    {
        Collidable = collidable;
        Move = move;
        OldPosition = new Position(collidable.Hitbox.Location.X, collidable.Hitbox.Location.Y);
        Hitbox = Collidable.Hitbox;
        NewHitbox = new RectangleF(move.Position.X, move.Position.Y, collidable.Hitbox.Width, collidable.Hitbox.Height);
    }

    public RectangleF Hitbox { get; }
    public RectangleF NewHitbox { get; }

    public ICollisionTrackable Collidable { get; }
    public Position OldPosition { get; }
    public Move Move { get; }
}
