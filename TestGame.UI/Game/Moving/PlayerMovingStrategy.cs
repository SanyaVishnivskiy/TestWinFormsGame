namespace TestGame.UI.Game.Moving
{
    internal class PlayerMovingStrategy : ILongMovable
    {
        private readonly HashSet<MoveDirection> _activeMovings = new();

        public Position CurrentPosition { get; }
        public MovingInfo Moving { get; }

        public PlayerMovingStrategy(Position currentPosition, MovingInfo movingInfo)
        {
            CurrentPosition = currentPosition;
            Moving = movingInfo;
        }

        public void FinishMoving(MoveDirection direction)
        {
            _activeMovings.Remove(direction);
        }

        public void StartMoving(MoveDirection direction)
        {
            _activeMovings.Add(direction);
        }

        public void Move()
        {
            if (_activeMovings.Contains(MoveDirection.Left))
            {
                CurrentPosition.AddX(-Moving.Speed);
            }
            
            if (_activeMovings.Contains(MoveDirection.Right))
            {
                CurrentPosition.AddX(Moving.Speed);
            }

            if (_activeMovings.Contains(MoveDirection.Up))
            {
                CurrentPosition.AddY(-Moving.Speed);
            }

            if (_activeMovings.Contains(MoveDirection.Down))
            {
                CurrentPosition.AddY(Moving.Speed);
            }
        }
    }
    
    public class MovingInfo
    {
        private float _speed;
        public float Speed
        {
            get => _speed * Constants.GameSpeed;
            set => _speed = value;
        }
    }
}
