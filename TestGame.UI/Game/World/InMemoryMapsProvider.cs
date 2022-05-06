namespace TestGame.UI.Game.World
{
    public interface IMapsProvider
    {
        string[] GetMapNames();
        IGameMap GetMapByName(string name);
    }

    internal class InMemoryMapsProvider : IMapsProvider
    {
        private static readonly Dictionary<string, GameMap> _maps = new List<GameMap> {
            new GameMap(
                "Default",
                new int [,] {
                    { 2, 2, 2, 2, 2, 2, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 1, 1, 1, 1, 1, 2 },
                    { 2, 2, 2, 2, 2, 2, 2 },
                },
                new int [,]
                {
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0 },
                },
                new Point(3, 7)
            )
        }.ToDictionary(x => x.Name);

        public string[] GetMapNames()
        {
            return _maps.Keys.ToArray();
        }

        public IGameMap GetMapByName(string name)
        {
            if (_maps.TryGetValue(name, out var map))
            {
                return map;
            }

            throw new ArgumentException("Unknown map");
        }
    }
}
