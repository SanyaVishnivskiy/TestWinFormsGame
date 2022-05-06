namespace TestGame.UI.Game.Animations
{
    internal class EntitiesAnimations
    {
        public static Animation HeroAnimations = Animation.New()
            .FromSprite(Textures.GhostAnimation)
            .WithFirstFrame(new Rectangle(0, 0, 32, 32))
            .WithFrameCount(4)
            .WithFrameDelay(TimeSpan.FromMilliseconds(500))
            .Looped()
            .Build();
    }
}
