namespace TestGame.UI.Game.Animations
{
    internal class AnimationsBuilder
    {
        private Bitmap? _sprites;
        private Rectangle _firstFrame;
        private int _frameCount = 1;
        private int _frameOffset;
        private TimeSpan _frameDelay;
        private bool _loop;

        public AnimationsBuilder FromSprite(Bitmap sprites)
        {
            _sprites = sprites;
            return this;
        }

        public AnimationsBuilder WithFirstFrame(Rectangle firstFrame)
        {
            _firstFrame = firstFrame;
            return this;
        }

        public AnimationsBuilder WithFrameCount(int frameCount)
        {
            _frameCount = frameCount;
            return this;
        }

        public AnimationsBuilder WithFrameOffset(int frameOffsetInPixels)
        {
            _frameOffset = frameOffsetInPixels;
            return this;
        }

        public AnimationsBuilder WithFrameDelay(TimeSpan frameDelay)
        {
            _frameDelay = frameDelay;
            return this;
        }

        public AnimationsBuilder Looped()
        {
            _loop = true;
            return this;
        }

        public Animation Build()
        {
            if (_sprites == null)
                throw new ArgumentNullException(nameof(_sprites));
            if (_firstFrame.Width == 0 || _firstFrame.Height == 0)
                throw new ArgumentException(nameof(_firstFrame));

            return new Animation(_sprites, _firstFrame, _frameCount, _frameOffset, _frameDelay, _loop);
        }
    }
}
