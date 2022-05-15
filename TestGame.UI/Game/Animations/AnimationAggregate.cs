namespace TestGame.UI.Game.Animations;

public class AnimationAggregate : IAnimation
{
    private readonly Dictionary<AnimationActionType, AnimationAggregateItem> _animations;

    public AnimationAggregate(List<AnimationAggregateItem> animations)
    {
        _animations = animations
            .ToDictionary(x => x.Animation.Type);
        _currentAnimation = animations
            .FirstOrDefault(x => x.Animation.Type == AnimationActionType.Idle)
            ?? animations[0];
    }

    private AnimationAggregateItem _currentAnimation;

    public Bitmap CurrentFrame => _currentAnimation.Animation.CurrentFrame;

    public Rectangle FirstFrame => _currentAnimation.Animation.FirstFrame;

    public Bitmap GetNextFrame()
    {
        return _currentAnimation.Animation.GetNextFrame();
    }

    public void ChangeAnimation(ChangeAnimationOptions options)
    {
        var typesToTry = new[] { options.ActionType }
            .Concat(options.FallbackAnimations)
            .ToList();

        foreach (var type in typesToTry)
        {
            if (_animations.ContainsKey(type))
            {
                _currentAnimation = _animations[type];
                _currentAnimation.Animation.Reset();
                return;
            }
        }
    }
}

public class AnimationAggregateItem
{
    public AnimationAggregateItem(Animation animation, bool interruptable)
    {
        Animation = animation;
        Interruptable = interruptable;
    }

    public Animation Animation { get; }
    public bool Interruptable { get; }
}

public struct ChangeAnimationOptions
{
    public AnimationActionType ActionType { get; }
    public TimeSpan Duration { get; init; } = default;
    public List<AnimationActionType> FallbackAnimations { get; init; } = new List<AnimationActionType>();

    public ChangeAnimationOptions(AnimationActionType type)
    {
        ActionType = type;
    }
}

public enum AnimationActionType
{
    None = 0,
    Idle,
    Death,
    MoveLeft,
    MoveRight,
    Attack,
    AttackDaggerLeft,
    AttackDaggerRight,
    AttackDaggerUp,
    AttackDaggerDown,
}