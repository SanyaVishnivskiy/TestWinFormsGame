namespace TestGame.UI.Game.Animations;

public interface IAnimationAggregatesBuilder
{
    AnimationAggregate BuildAggregation();
    IAnimationAggregatesBuilder NewAnimation(Func<IAnimationsBuilder, IAnimationsBuilder> builder);
    IAnimationAggregatesBuilder OfType(AnimationActionType type);
}

internal class AnimationsBuilderItem
{
    public IAnimationsBuilder Builder { get; set; }
    public AnimationActionType Type { get; set; }
}

public class AnimationAggregatesBuilder : IAnimationAggregatesBuilder
{
    private readonly List<AnimationsBuilderItem> _builders = new();
    private int _currentBuilderIndex = -1;
    private AnimationsBuilderItem CurrentBuilder => _builders[_currentBuilderIndex];

    public IAnimationAggregatesBuilder NewAnimation(Func<IAnimationsBuilder, IAnimationsBuilder> build)
    {
        var builder = build(new AnimationsBuilder());
        _builders.Add(new AnimationsBuilderItem
        {
            Builder = builder
        });
        _currentBuilderIndex++;
        return this;
    }

    public IAnimationAggregatesBuilder OfType(AnimationActionType type)
    {
        if (CurrentBuilder.Builder is IAggregatedAnimationBuilder builder)
        {
            builder.OfType(type);
        }
        return this;
    }

    public AnimationAggregate BuildAggregation()
    {
        var animations = new List<AnimationAggregateItem>();
        foreach (var builder in _builders)
        {
            animations.Add(new AnimationAggregateItem(builder.Builder.Build(), true)); //TODO fix true
        }

        return new AnimationAggregate(animations);
    }
}
