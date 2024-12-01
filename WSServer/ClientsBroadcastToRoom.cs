using Fleck;
using lib;
using System.Text.Json;

namespace WSServer
{
    public class ClientsBroadcastToRoomDto : BaseDto
    {
        public string message { get; set; }
        public int roomId { get; set; }
    }
    public class ClientsBroadcastToRoom : BaseEventHandler<ClientsBroadcastToRoomDto>
    {
        public override Task Handle(ClientsBroadcastToRoomDto dto, IWebSocketConnection socket)
        {
            var message = new ServerBroadcastMessageWithUsername()
            {
                message = dto.message,
                username = StateService.Connections[socket.ConnectionInfo.Id].Username,
            };
            StateService.BroadCastToRoom(dto.roomId, JsonSerializer.Serialize(message));
            return Task.CompletedTask;
        }
    }
}
