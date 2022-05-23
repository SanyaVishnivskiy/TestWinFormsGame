namespace TestGame.UI.Game.Animations
{
    public static class WeaponAnimations
    {
        public static AnimationAggregate MeleeDefaultAnimation = Animation.NewAggregate()
            .NewAnimation(b => b
                .FromSprite(Textures.Dagger)
                .WithFirstFrame(new Rectangle(0, 0, 8, 24))
                .FlipHorizontally())
            .OfType(AnimationActionType.Attack)
            .ForDirection(MoveDirection.Up)
            .NewAnimation(b => b
                .FromSprite(Textures.Dagger)
                .WithFirstFrame(new Rectangle(0, 0, 8, 24))
                .FlipVertically())
            .OfType(AnimationActionType.Attack)
            .ForDirection(MoveDirection.Down)
            .NewAnimation(b => b
                .FromSprite(Textures.Dagger)
                .WithFirstFrame(new Rectangle(0, 0, 8, 24))
                .RotateLeft())
            .OfType(AnimationActionType.Attack)
            .ForDirection(MoveDirection.Left)
            .NewAnimation(b => b
                .FromSprite(Textures.Dagger)
                .WithFirstFrame(new Rectangle(0, 0, 8, 24))
                .RotateLeft()
                .FlipHorizontally())
            .OfType(AnimationActionType.Attack)
            .ForDirection(MoveDirection.Right)
            .BuildAggregation();
    }
}
