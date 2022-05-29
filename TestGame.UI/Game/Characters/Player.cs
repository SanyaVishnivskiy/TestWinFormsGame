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
        Health = new Health(6, 10, new Size(Width, Height));
    }

    public void StartMoving(MoveDirection direction)
    {
        WalkableBehaviour.StartMoving(direction);
    }

    public void FinishMoving(MoveDirection direction)
    {
        WalkableBehaviour.FinishMoving(direction);
    }

    public override bool IsEnemy(Entity e)
    {
        return e is Enemy;
    }
}
