namespace TestGame.UI.Game.Physics.Collisions;

public interface ICollidable
{
    RectangleF Hitbox { get; }
    void OnCollision(ICollidable other);
}
