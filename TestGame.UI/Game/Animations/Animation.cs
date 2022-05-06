namespace TestGame.UI.Game.Animations
{
    internal class Animation
    {
        private readonly List<Bitmap> frames = new();

        private int _currentFrame = 0;
        private bool _playedOnce = false;
        private DateTime _lastFrameUpdated = DateTime.MinValue;
        
        public Animation(Bitmap image, Rectangle firstFrame, int frameCount, int nextFrameOffset, TimeSpan frameDelay, bool loop)
        {
            Image = image;
            FirstFrame = firstFrame;
            FrameCount = frameCount;
            NextFrameOffset = nextFrameOffset;
            FrameDelay = frameDelay;
            Loop = loop;

            InitFrames();
        }

        public Bitmap Image { get; }
        public Rectangle FirstFrame { get; }
        public int FrameCount { get; }
        public int NextFrameOffset { get; }
        public TimeSpan FrameDelay { get; }
        public bool Loop { get; }

        public Bitmap CurrentFrame => frames[_currentFrame];

        private void InitFrames()
        {
            for (int i = 0; i < FrameCount; i++)
            {
                frames.Add(CropFrame(i));
            }
        }

        private Bitmap CropFrame(int i)
        {
            var frame = new Bitmap(FirstFrame.Width, FirstFrame.Height);
            using var graphics = Graphics.FromImage(frame);

            var offset = i > 0 ? NextFrameOffset : 0;
            var frameX = FirstFrame.X + ((FirstFrame.Width + offset) * i);
            var cropRect = new Rectangle(frameX, FirstFrame.Y, frame.Width, frame.Height);

            graphics.DrawImage(Image, new Rectangle(0, 0, frame.Width, frame.Height), cropRect, GraphicsUnit.Pixel);

            return frame;
        }

        public Bitmap GetNextFrame()
        {
            if (_playedOnce && !Loop)
            {
                return frames[0];
            }

            if (DateTime.Now - _lastFrameUpdated > FrameDelay)
            {
                _currentFrame++;
                _lastFrameUpdated = DateTime.Now;
            }

            if (_currentFrame >= FrameCount)
            {
                _currentFrame = 0;
                _playedOnce = true;
            }

            return CurrentFrame;
        }

        public static AnimationsBuilder New() => new AnimationsBuilder();
    }
}
