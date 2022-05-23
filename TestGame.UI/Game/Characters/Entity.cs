namespace TestGame.UI.Game.Characters;

public abstract class Entity : IRenderable, IMovable, IAttackable
{

    protected IMovable MovableBehaviour { get; set; }

    public Entity(Position position, AnimationAggregate animation)
    {
        Position = position;
        Animation = animation;
        Width = Animation.FirstFrame.Width;
        Height = Animation.FirstFrame.Height;
    }

    public Position Position { get; }
    public Position CurrentPosition => Position;
    public Direction FaceDirection { get; protected set; }
    public Weapon Weapon { get; set; }

    public AnimationAggregate Animation { get; }
    public MovingInfo Moving { get; protected set; }

    public int Width { get; protected set; }
    public int Height { get; protected set; }
    public Bitmap CurrentTexture => Animation.CurrentFrame;

    public void EnsureInitialized()
    {
        if (MovableBehaviour is null)
        {
            throw new ArgumentNullException(nameof(MovableBehaviour));
        }
        if (Moving is null)
        {
            throw new ArgumentNullException(nameof(Moving));
        }
        if (Width == 0)
        {
            throw new ArgumentNullException(nameof(Width));
        }
        if (Height == 0)
        {
            throw new ArgumentNullException(nameof(Height));
        }
        if (Animation is null)
        {
            throw new ArgumentNullException(nameof(Animation));
        }
        if (Position is null)
        {
            throw new ArgumentNullException(nameof(Position));
        }
        EnsureEntityUniqueInitialized();
    }

    protected virtual void EnsureEntityUniqueInitialized()
    {
    }
    
    public Bitmap GetTexture()
    {
        return Animation.GetNextFrame();
    }

    public Move GetNewMove()
    {
        return MovableBehaviour.GetNewMove();
    }

    public void AdjustMovementOnce(MoveAdjustment direction)
    {
        MovableBehaviour.AdjustMovementOnce(direction);
    }

    public Move Move()
    {
        var move = MovableBehaviour.Move();
        UpdateMoveAnimation(move);
        FaceDirection = move.Direction;
        return move;
    }

    public void UpdateMoveAnimation(Move move)
    {
        if (Weapon?.Attacking ?? false)
        {
            return;
        }

        var options = new ChangeAnimationOptions(AnimationActionType.Move)
        {
            FallbackAnimations =
            {
                AnimationActionType.Idle
            },
            Direction = move.Direction.Horizontal
        };

        Animation.ChangeAnimation(options);
    }

    public void Attack()
    {
        if (Weapon is null)
        {
            return;
        }

        var details = Weapon.TryBeginAttack(this);
        if (details.AttackStarted)
        {
            StartAttackAnimation();
            GameState.Instance.MarkAttacking(this);
        }
    }

    private void StartAttackAnimation()
    {
        var options = new ChangeAnimationOptions(AnimationActionType.Attack)
        {
            Direction = GetAttackAnimation(),
        };

        Animation.ChangeAnimation(options);
        Weapon.Animation.ChangeAnimation(options);
    }

    private MoveDirection GetAttackAnimation()
    {
        if (FaceDirection.IsDiagonal || FaceDirection.IsHorizontal)
        {
            return FaceDirection.Horizontal;
        }

        return FaceDirection.Vertical;
    }

    public void AttackTick()
    {
        if (Weapon is null)
        {
            return;
        }
        
        var details = Weapon.AttackTick(this);
        if (details.AttackFinished)
        {
            EndAttackAnimation();
            GameState.Instance.MarkAttackFinished(this);
        }
    }

    private void EndAttackAnimation()
    {
        Animation.ReturnToAnimation(AnimationActionType.Move);
    }
}
