namespace TestGame.UI.Game.Physics.Collisions;

public class OverridenCollidableAdapter : ICollidable
{
    public OverridenCollidableAdapter(ICollidable collidable, Move move)
    {
        Collidable = collidable;
        Move = move;
        OldPosition = new Position(collidable.Hitbox.Location.X, collidable.Hitbox.Location.Y);
        NewHitbox = new RectangleF(move.Position.X, move.Position.Y, collidable.Hitbox.Width, collidable.Hitbox.Height);
    }

    public RectangleF Hitbox => Collidable.Hitbox;
    public RectangleF NewHitbox { get; }

    public ICollidable Collidable { get; }
    public Position OldPosition { get; }
    public Move Move { get; }
}
