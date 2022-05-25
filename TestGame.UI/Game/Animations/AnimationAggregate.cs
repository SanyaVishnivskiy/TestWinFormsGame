namespace TestGame.UI.Game.Animations;

public class AnimationAggregate : IAnimation
{
    private readonly Dictionary<AnimationAggregateItemKey, AnimationAggregateItem> _animations;
    private readonly Dictionary<AnimationActionType, AnimationAggregateItem> _previousAnimations;

    public AnimationAggregate(List<AnimationAggregateItem> animations)
    {
        _animations = animations
            .ToDictionary(x => new AnimationAggregateItemKey(x.Type, x.Direction));
        _currentAnimation = animations
            .FirstOrDefault(x => x.Animation.Type == AnimationActionType.Idle)
            ?? animations[0];
        _previousAnimations = new();
        Default = _currentAnimation.Animation;
    }

    private AnimationAggregateItem _currentAnimation;

    public Bitmap CurrentFrame => _currentAnimation.Animation.CurrentFrame;
    public Animation CurrentAnimation => _currentAnimation.Animation;
    public Animation Default { get; }

    public Rectangle FirstFrame => Default.FirstFrame;

    public void ReturnToAnimation(AnimationActionType type)
    {
        if (_previousAnimations.TryGetValue(type, out var previousAnimation))
        {
            _currentAnimation = previousAnimation;
        }
    }

    public Bitmap GetNextFrame()
    {
        return _currentAnimation.Animation.GetNextFrame();
    }

    public void ChangeAnimation(ChangeAnimationOptions options)
    {
        if (MatchCurrentAnimation(options))
        {
            return;
        }

        var typesToTry = new List<AnimationActionType>(options.FallbackAnimations.Count + 2);
        typesToTry.Add(options.ActionType);
        typesToTry.AddRange(options.FallbackAnimations);
        typesToTry.Add(Default.Type);

        var current = _currentAnimation;
        foreach (var type in typesToTry)
        {
            var key = new AnimationAggregateItemKey(type, options.Direction);
            if (_animations.ContainsKey(key))
            {
                _currentAnimation = _animations[key];
                _currentAnimation.Animation.Reset();
                _previousAnimations[current.Type] = current;
                return;
            }
        }
    }

    private bool MatchCurrentAnimation(ChangeAnimationOptions options)
    {
        return options.ActionType == _currentAnimation.Type
            && options.Direction == _currentAnimation.Direction;
    }
}

public record AnimationAggregateItemKey(AnimationActionType Type, MoveDirection? Direction);

public class AnimationAggregateItem
{
    public AnimationAggregateItem(Animation animation)
    {
        Animation = animation;
    }

    public Animation Animation { get; }
    public bool Interruptable { get; init; }
    public AnimationActionType Type { get; init; }
    public MoveDirection? Direction { get; init; }
}

public struct ChangeAnimationOptions
{
    public AnimationActionType ActionType { get; }
    public TimeSpan Duration { get; init; } = default;
    public List<AnimationActionType> FallbackAnimations { get; init; } = new();
    public MoveDirection? Direction { get; init; } = null;

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
    Move,
    Attack,
}