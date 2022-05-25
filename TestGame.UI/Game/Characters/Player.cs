namespace TestGame.UI.Game.Characters;

public class Player : Entity, IWalkable, ICollidable
{
    protected IWalkable WalkableBehaviour => (MovableBehaviour as IWalkable)!;

    public RectangleF Hitbox => new RectangleF(Position.X, Position.Y, Width, Height);

    public Player(Position position) : base(position, EntitiesAnimations.HeroAnimations)
    {
        Moving = new MovingInfo {
            Speed = 150,
        };
        MovableBehaviour = new PlayerMovingBehaviour(Position, Moving);
    }

    public void StartMoving(MoveDirection direction)
    {
        WalkableBehaviour.StartMoving(direction);
    }

    public void FinishMoving(MoveDirection direction)
    {
        WalkableBehaviour.FinishMoving(direction);
    }
}
