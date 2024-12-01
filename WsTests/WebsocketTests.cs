using lib;
using Websocket.Client;
using WSServer;

namespace WsTests
{
    public class WebsocketTests
    {
        public WebsocketTests()
        {
            Startup.StartupExecute(default);
        }

        [Fact]
        public async Task WebsocketTest()
        {
            
            var ws = await new WebSocketTestClient().ConnectAsync();
            await ws.DoAndAssert(new ClientToServerDto() 
            {
                MessageContent = "hey"
            }, fromServer => 
            {
                return fromServer.Count(dto => dto.eventType == nameof(ServerEchoClient)) == 1;
            });
        }
    }
}