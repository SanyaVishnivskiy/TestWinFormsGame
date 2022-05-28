using System.Diagnostics;

namespace TestGame.UI.Common
{
    public static class Logger
    {
        private static List<ILoggerDestination> _destinations = new();
        private static readonly List<Func<LogCategory, string, bool>> _logFilters = new();
        private static Stopwatch _stopwatch = new();

        static Logger()
        {
            _logFilters.Add((category, _) => category == LogCategory.PrintAlways);
        }

        public static void Log(LogCategory category, string message)
        {
            if (!ShouldBePrinted(category, message))
            {
                return;
            }

            foreach (var destination in _destinations)
            {
                destination.Log(message);
            }
        }

        private static bool ShouldBePrinted(LogCategory category, string message)
        {
            return _logFilters.Any(filter => filter(category, message));
        }

        public static void Messure(string action, Action actionToMeasure)
        {
            _stopwatch.Restart();
            actionToMeasure();
            _stopwatch.Stop();

            Log(LogCategory.Messuring, $"{action} took {_stopwatch.ElapsedMilliseconds}ms");
        }

        public static void AddDestination(ILoggerDestination destination)
        {
            _destinations.Add(destination);
        }

        public static void AddLoggerFilter(Func<LogCategory, string, bool> filter)
        {
            _logFilters.Add(filter);
        }
    }

    public interface ILoggerDestination
    {
        void Log(string message);
    }
}

public enum LogCategory
{
    PrintAlways,
    Movement,
    Rendering,
    Messuring,
    Attack,
}
