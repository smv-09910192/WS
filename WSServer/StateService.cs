using Fleck;

namespace WSServer
{
    public class WebSocketMetaData(IWebSocketConnection connection)
    {
        public IWebSocketConnection Connection { get; private set; } = connection;
        public string Username { get; set; }
    }

    public static class StateService
    {
        public static Dictionary<Guid, WebSocketMetaData> Connections = new();
        public static Dictionary<int, HashSet<Guid>> Rooms = new();
        public static void AddConnection(IWebSocketConnection c)
        {
            Connections.TryAdd(c.ConnectionInfo.Id, new WebSocketMetaData(c));
        }

        public static bool AddToRoom(IWebSocketConnection c, int room)
        {
            if (!Rooms.ContainsKey(room))
            {
                Rooms.Add(room, new HashSet<Guid>());
            }
            return Rooms[room].Add(c.ConnectionInfo.Id);
        }

        public static void BroadCastToRoom(int room, string message)
        {
            if (Rooms.TryGetValue(room, out var guids))
            {
                foreach (var item in guids)
                {
                    if (Connections.TryGetValue(item, out var ws))
                    {
                        ws.Connection.Send(message);
                    }
                }
            }
        }
    }
}