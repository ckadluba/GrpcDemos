using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace MinimalGoogleGrpc.Service
{
    public class Program
    {
        private const string Host = "localhost";
        private const int Port = 5001;

        public static async Task Main()
        {
            // Use Google Grpc.Core
            EnableGoogleGrpcLogging();

            Server server = new Server();
            server.Services.Add(Greeter.BindService(new GreeterService()));
            server.Ports.Add(new ServerPort(Host, Port, ServerCredentials.Insecure));
            //server.Ports.Add(new ServerPort(Host, Port, new SslServerCredentials(...)));  // With TLS
            server.Start();

            Console.WriteLine($"The service is ready at " +
                              $"{Host} on port {Port}.");
            Console.WriteLine("Press any key to stop the " +
                      " service...");
            Console.ReadLine();

            await server.ShutdownAsync();
        }

        private static void EnableGoogleGrpcLogging()
        {
            // Google Grpc.Core logging
            Environment.SetEnvironmentVariable("GRPC_TRACE", "api");
            Environment.SetEnvironmentVariable("GRPC_VERBOSITY", "debug");
            Grpc.Core.GrpcEnvironment.SetLogger(new Grpc.Core.Logging.ConsoleLogger());
        }
    }
}
