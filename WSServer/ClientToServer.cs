using Fleck;
using lib;
using System.Text.Json;

namespace WSServer
{
    public class ClientToServerDto : BaseDto
    {
        public string MessageContent { get; set; }
    }

    public class ClientToServer : BaseEventHandler<ClientToServerDto>
    {
        public override Task Handle(ClientToServerDto dto, IWebSocketConnection socket)
        {
            var echo = new ServerEchoClient()
            {
                echoValue = $"echo: = {dto.MessageContent}"
            };
            var messageToClient = JsonSerializer.Serialize(echo);
            socket.Send(messageToClient);
            return Task.CompletedTask;
        }
    }

    public class ServerEchoClient : BaseDto
    {
        public string echoValue { get; set; }
    }
}
