namespace TestGame.UI.Game.Physics.Collisions;

public interface ICollisionTrackable
{
    RectangleF Hitbox { get; }
}

public interface ICollidable : ICollisionTrackable
{
}
