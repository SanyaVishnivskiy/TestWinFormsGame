namespace TestGame.UI.Game.World
{
    public class MapObject : IRenderable
    {
        public MapObject(Position position, MapObjectType type)
        {
            Position = position;
            Type = type;
        }

        public Position Position { get; set; }
        public MapObjectType Type { get; }

        public Bitmap GetFrame()
        {
            return MapObjectTextureMap.GetTexture(Type);
        }
    }

    public enum MapObjectType
    {
        None = 0,
    }


    internal class MapObjectTextureMap
    {
        public static Dictionary<MapObjectType, Bitmap> _tileTypeTextureMap = new()
        {
        };

        public static Bitmap GetTexture(MapObjectType type)
        {
            return _tileTypeTextureMap[type];
        }
    }
}
