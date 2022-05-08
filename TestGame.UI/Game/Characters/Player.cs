namespace TestGame.UI.Game.Characters;

internal class Player : Entity, IWalkable
{
    private readonly IWalkable _movable;

    public MovingInfo Moving { get; }

    public Player(Position position) : base(position, EntitiesAnimations.HeroAnimations)
    {
        Moving = new MovingInfo {
            Speed = 20,
        };
        _movable = new PlayerMovingStrategy(Position, Moving);
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
}
