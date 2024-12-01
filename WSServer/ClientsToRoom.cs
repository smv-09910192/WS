using Fleck;
using lib;
using System.Text.Json;

namespace WSServer
{
    public class ClientsToRoomDto : BaseDto
    {
        public int roomId { get; set; }
    }
    public class ClientsToRoom : BaseEventHandler<ClientsToRoomDto>
    {
        public override Task Handle(ClientsToRoomDto dto, IWebSocketConnection socket)
        {
            var isSuccess = StateService.AddToRoom(socket, dto.roomId);
            socket.Send(JsonSerializer.Serialize(new ServerAddsClientToRoom()
            {
                message = $"You were successfully added to room with ID {dto.roomId}"
            }));
            return Task.CompletedTask;
        }
    }

    public class ServerAddsClientToRoom : BaseDto
    {
        public string message { get; set; }
    }
}
