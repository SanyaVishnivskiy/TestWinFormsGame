namespace TestGame.UI.Common
{
    public static class Logger
    {
        private static List<ILoggerDestination> _destinations = new();

        public static void Log(string message)
        {
            foreach (var destination in _destinations)
            {
                destination.Log(message);
            }
        }

        public static void AddDestination(ILoggerDestination destination)
        {
            _destinations.Add(destination);
        }
    }

    public interface ILoggerDestination
    {
        void Log(string message);
    }
}
