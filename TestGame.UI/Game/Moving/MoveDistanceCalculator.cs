namespace TestGame.UI.Game.Moving;

public class MoveDistanceCalculator
{
    public static float Calculate(float distance)
    {
        var result = (float)Math.Round(distance * Time.DeltaTime);
        Logger.Log(LogCategory.Movement, $"Distance {distance} calculated result {result}");
        return result;
    }
}
