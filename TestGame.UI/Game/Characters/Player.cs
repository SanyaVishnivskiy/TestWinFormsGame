namespace TestGame.UI.Game.Characters;

internal class Player : Entity, IWalkable, ICollidable
{
    private readonly IWalkable _movable;

    public MovingInfo Moving { get; }

    public RectangleF Hitbox => new RectangleF(Position.X, Position.Y, Animation.CurrentFrame.Width, Animation.CurrentFrame.Height);

    public Player(Position position) : base(position, EntitiesAnimations.HeroAnimations)
    {
        Moving = new MovingInfo {
            Speed = 20,
        };
        _movable = new PlayerMovingStrategy(Position, Moving);
    }

    public Position GetNewMove()
    {
        return _movable.GetNewMove();
    }

    public void DenyMoveToDirectionOnce(MoveDirection direction)
    {
        _movable.DenyMoveToDirectionOnce(direction);
    }

    public void Move()
    {
        _movable.Move();
    }

    public void StartMoving(MoveDirection direction)
    {
        _movable.StartMoving(direction);
    }

    public void FinishMoving(MoveDirection direction)
    {
        _movable.FinishMoving(direction);
    }

    public void OnCollision(ICollidable other)
    {
        throw new NotImplementedException();
    }
}
