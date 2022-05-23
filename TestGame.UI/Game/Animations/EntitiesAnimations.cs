namespace TestGame.UI.Game.Animations
{
    internal class EntitiesAnimations
    {
        #region Player
        public static AnimationAggregate HeroAnimations = Animation.NewAggregate()
            .NewAnimation(b => b
                .FromSprite(Textures.GhostAnimation)
                .WithFirstFrame(new Rectangle(0, 0, 32, 32))
                .WithFrameCount(4)
                .WithFrameDelay(TimeSpan.FromMilliseconds(500))
                .Looped())
            .OfType(AnimationActionType.Move)
            .ForDirection(MoveDirection.Right)
            .NewAnimation(b => b
                .FromSprite(Textures.GhostAnimation)
                .WithFirstFrame(new Rectangle(0, 32, 32, 32))
                .WithFrameCount(4)
                .WithFrameDelay(TimeSpan.FromMilliseconds(500))
                .Looped())
            .OfType(AnimationActionType.Move)
            .ForDirection(MoveDirection.Left)
            .NewAnimation(b => b
                .FromSprite(Textures.GhostAnimation)
                .WithFirstFrame(new Rectangle(0, 64, 32, 32))
                .WithFrameCount(4)
                .WithFrameDelay(TimeSpan.FromMilliseconds(200))
                .Looped())
            .OfType(AnimationActionType.Attack)
            .ForDirection(MoveDirection.Left)
            .NewAnimation(b => b
                .FromSprite(Textures.GhostAnimation)
                .WithFirstFrame(new Rectangle(0, 96, 32, 32))
                .WithFrameCount(4)
                .WithFrameDelay(TimeSpan.FromMilliseconds(200))
                .Looped())
            .OfType(AnimationActionType.Attack)
            .ForDirection(MoveDirection.Right)
            .NewAnimation(b => b
                .FromSprite(Textures.GhostAnimation)
                .WithFirstFrame(new Rectangle(0, 128, 32, 32))
                .WithFrameCount(4)
                .WithFrameDelay(TimeSpan.FromMilliseconds(200))
                .Looped())
            .OfType(AnimationActionType.Attack)
            .ForDirection(MoveDirection.Up)
            .NewAnimation(b => b
                .FromSprite(Textures.GhostAnimation)
                .WithFirstFrame(new Rectangle(0, 160, 32, 32))
                .WithFrameCount(4)
                .WithFrameDelay(TimeSpan.FromMilliseconds(200))
                .Looped())
            .OfType(AnimationActionType.Attack)
            .ForDirection(MoveDirection.Down)
            .BuildAggregation();
        #endregion Player

        public static AnimationAggregate DummyAnimation = Animation.NewAggregate()
            .NewAnimation(b => b
                .FromSprite(Textures.Dummy))
            .BuildAggregation();
    }
}
