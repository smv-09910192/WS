
using Fleck;
using lib;
using System.Reflection;
using WSServer;

public static class Startup
{
    public static void Main(string[] args)
    {
        StartupExecute(args);
        Console.ReadLine();
    }

    public static void StartupExecute(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var clientEventhandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

        var server = new WebSocketServer("ws://0.0.0.0:8181");
        var wsConnections = new List<IWebSocketConnection>();

        var app = builder.Build();

        server.Start(c => {
            c.OnOpen = () =>
            {
                StateService.AddConnection(c);
            };
            c.OnMessage = message =>
            {
                try
                {
                    app.InvokeClientEventHandler(clientEventhandlers, c, message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine(ex.StackTrace);
                }
            };
        });

        //app.Run();
    }
}

