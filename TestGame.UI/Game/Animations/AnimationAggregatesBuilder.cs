namespace TestGame.UI.Game.Animations;

public interface IAnimationAggregatesBuilder
{
    AnimationAggregate BuildAggregation();
    IAnimationAggregatesBuilder NewAnimation(Func<IAnimationsBuilder, IAnimationsBuilder> builder);
    IAnimationAggregatesBuilder OfType(AnimationActionType type);
    IAnimationAggregatesBuilder ForDirection(MoveDirection? Direction);
}

internal class AnimationsBuilderItem
{
    public IAnimationsBuilder? Builder { get;}
    public AnimationActionType Type { get; set; }
    public MoveDirection? Direction { get; set; }

    public AnimationsBuilderItem(IAnimationsBuilder? builder)
    {
        Builder = builder;
    }
}

public class AnimationAggregatesBuilder : IAnimationAggregatesBuilder
{
    private readonly List<AnimationsBuilderItem> _builders = new();
    private int _currentBuilderIndex = -1;
    private AnimationsBuilderItem CurrentBuilder => _builders[_currentBuilderIndex];

    public IAnimationAggregatesBuilder NewAnimation(Func<IAnimationsBuilder, IAnimationsBuilder> build)
    {
        var builder = build(new AnimationsBuilder());
        _builders.Add(new AnimationsBuilderItem(builder));
        _currentBuilderIndex++;
        return this;
    }

    public IAnimationAggregatesBuilder OfType(AnimationActionType type)
    {
        CurrentBuilder.Type = type;
        if (CurrentBuilder.Builder is IAggregatedAnimationBuilder builder)
        {
            builder.OfType(type);
        }

        return this;
    }

    public IAnimationAggregatesBuilder ForDirection(MoveDirection? Direction)
    {
        CurrentBuilder.Direction = Direction;
        return this;
    }

    public AnimationAggregate BuildAggregation()
    {
        var animations = new List<AnimationAggregateItem>();
        foreach (var builder in _builders)
        {
            animations.Add(new AnimationAggregateItem(builder.Builder.Build())
            {
                Interruptable = true,
                Direction = builder.Direction,
                Type = builder.Type
            }); //TODO fix true
        }

        return new AnimationAggregate(animations);
    }
}
