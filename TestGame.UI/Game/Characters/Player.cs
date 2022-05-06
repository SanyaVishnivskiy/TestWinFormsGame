namespace TestGame.UI.Game.Characters
{
    internal class Player : IWalkable, IRenderable
    {
        private readonly IWalkable _movable;

        public Animation Animation { get; }
        public Position Position { get; }
        public MovingInfo Moving { get; }

        public Player(Position position, Bitmap heroAnimations)
        {
            Position = position;
            Animation = new Animation(heroAnimations, new Rectangle(0, 0, 32, 32), 4, 0, TimeSpan.FromMilliseconds(500), true);
            Moving = new MovingInfo {
                Speed = 20,
            };
            _movable = new PlayerMovingStrategy(Position, Moving);
        }

        public Bitmap GetFrame()
        {
            return Animation.GetNextFrame();
        }

        public void Move()
        {
            _movable.Move();
        }

        public void StartMoving(MoveDirection direction)
        {
            _movable.StartMoving(direction);
        }

        public void FinishMoving(MoveDirection direction)
        {
            _movable.FinishMoving(direction);
        }
    }
}
