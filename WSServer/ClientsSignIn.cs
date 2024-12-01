using Fleck;
using lib;
using System.Text.Json;

namespace WSServer
{
    public class ClientsSignInDto : BaseDto
    {
        public string Username { get; set; }
    }
    public class ClientsSignIn : BaseEventHandler<ClientsSignInDto>
    {
        public override Task Handle(ClientsSignInDto dto, IWebSocketConnection socket)
        {
            StateService.Connections[socket.ConnectionInfo.Id].Username = dto.Username;
            socket.Send(JsonSerializer.Serialize(new ServerWelcomeUser()));
            return Task.CompletedTask;
        }
    }

    public class ServerWelcomeUser : BaseDto;

    public class ServerBroadcastMessageWithUsername : BaseDto
    {
        public string message { get; set; }
        public string username { get; set; }
    }
}
