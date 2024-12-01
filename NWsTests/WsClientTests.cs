using lib;
using WSServer;

namespace NWsTests
{
    public class WsClientTests
    {
        [SetUp]
        public void Setup()
        {
            Startup.StartupExecute(default);
        }

        [Test]
        public async Task WsClientTest()
        {
            var ws = await new WebSocketTestClient().ConnectAsync();
            var ws2 = await new WebSocketTestClient().ConnectAsync();
            await ws.DoAndAssert(new ClientsSignInDto()
            {
                Username = "Max"
            }, result => result.Count(dto => dto.eventType == nameof(ServerWelcomeUser)) == 1) ;
            await ws2.DoAndAssert(new ClientsSignInDto()
            {
                Username = "Lilia"
            }, result => result.Count(dto => dto.eventType == nameof(ServerWelcomeUser)) == 1);

            await ws.DoAndAssert(new ClientsToRoomDto()
            {
                roomId = 1
            }, result => result.Count(dto => dto.eventType == nameof(ServerAddsClientToRoom)) == 1);
            await ws2.DoAndAssert(new ClientsToRoomDto()
            {
                roomId = 1
            }, result => result.Count(dto => dto.eventType == nameof(ServerAddsClientToRoom)) == 1);

            await ws.DoAndAssert(new ClientsBroadcastToRoomDto()
            {
                roomId = 1,
                message = "hi Liliia"
            }, result => result.Count(dto => dto.eventType == nameof(ServerBroadcastMessageWithUsername)) == 1);
            await ws2.DoAndAssert(new ClientsBroadcastToRoomDto()
            {
                roomId = 1,
                message = "hi Max"
            }, result => result.Count(dto => dto.eventType == nameof(ServerBroadcastMessageWithUsername)) == 2);
        }
    }
}