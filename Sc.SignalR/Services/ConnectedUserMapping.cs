namespace Sc.SignalR.Services
{
    public static class ConnectionUserMapping
    {
        private static readonly Dictionary<int, HashSet<string>> allConnections =
            new Dictionary<int, HashSet<string>>();

        public static IEnumerable<string> GetConnections(int key)
        {
            HashSet<string> connections;
            if (allConnections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public static void Add(int key, string connectionId)
        {
            lock (allConnections)
            {
                HashSet<string> connections;
                if (!allConnections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    allConnections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public static void Remove(int key, string connectionId)
        {
            lock (allConnections)
            {
                HashSet<string> connections;
                if (!allConnections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        allConnections.Remove(key);
                    }
                }
            }
        }
    }
}
