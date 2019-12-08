using System;
using System.Threading.Tasks;
using Grpc.Core;
using LifeTime.ServiceLib;

namespace LifeTime.ServiceHostGoogleGrpc
{
    public class Program
    {
        private const string Host = "localhost";
        private const int Port = 5003;

        public static async Task Main()
        {
            Server server = new Server();
            server.Services.Add(SharedLib.LifeTimeService.BindService(new LifeTimeService()));
            server.Ports.Add(new ServerPort(Host, Port, ServerCredentials.Insecure));
            server.Start();

            Console.WriteLine($"The service is ready at " +
                              $"{Host} on port {Port}.");
            Console.WriteLine("Press any key to stop the " +
                      " service...");
            Console.ReadLine();

            await server.ShutdownAsync();
        }
    }
}
