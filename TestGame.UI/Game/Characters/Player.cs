namespace TestGame.UI.Game.Characters;

public class Player : Entity, IWalkable, ICollidable
{
    private readonly IWalkable _movable;

    public MovingInfo Moving { get; }
    public Position CurrentPosition => Position;

    public RectangleF Hitbox => new RectangleF(Position.X, Position.Y, Width, Height);

    public Player(Position position) : base(position, EntitiesAnimations.HeroAnimations)
    {
        Moving = new MovingInfo {
            Speed = 20,
        };
        _movable = new PlayerMovingStrategy(Position, Moving);
    }

    public Move GetNewMove()
    {
        return _movable.GetNewMove();
    }

    public void AdjustMovementOnce(MoveAdjustment direction)
    {
        _movable.AdjustMovementOnce(direction);
    }

    public Move Move()
    {
        var move = _movable.Move();
        UpdateMoveAnimation(move);
        return move;
    }

    private void UpdateMoveAnimation(Move move)
    {
        if (!move.Direction.IsHorizontal)
        {
            return;
        }

        if (move.Direction.Horizontal == MoveDirection.Left)
        {
            Animation.ChangeAnimation(new ChangeAnimationOptions(AnimationActionType.MoveLeft));
        }
        else
        {
            Animation.ChangeAnimation(new ChangeAnimationOptions(AnimationActionType.MoveRight));
        }
    }

    public void StartMoving(MoveDirection direction)
    {
        _movable.StartMoving(direction);
    }

    public void FinishMoving(MoveDirection direction)
    {
        _movable.FinishMoving(direction);
    }
}
