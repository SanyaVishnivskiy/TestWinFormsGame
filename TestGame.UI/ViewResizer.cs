namespace TestGame.UI
{
    public class ViewResizer
    {
        public static Position CalculatePosition(Position position)
        {
            var newX = CalculateWidthProportion(position.X);
            var newY = CalculateHeightProportion(position.Y);
            return new Position(newX, newY);
        }
        
        public static Size CalculateTextureSize(IRenderable entity)
        {
            return new Size(
                (int)CalculateWidthProportion(entity.CurrentTextureWidth),
                (int)CalculateHeightProportion(entity.CurrentTextureHeight));
        }

        public static float CalculateWidthProportion(float value)
        {
            return value * GameState.Instance.Camera.ClientSize.Width / Constants.DefaultWindowWidth;
        }

        public static float CalculateHeightProportion(float value)
        {
            return value * GameState.Instance.Camera.ClientSize.Height / Constants.DefaultWindowHeight;
        }
    }
}
