namespace TestGame.UI.Game.Moving;

public class MovingInfo
{
    private float _speed;
    public float Speed
    {
        get => _speed * Constants.GameSpeed;
        set => _speed = value;
    }

    private static readonly MovingInfo _noneInstance = new MovingInfo
    {
        Speed = 0
    };
    public static MovingInfo None => _noneInstance;
}
