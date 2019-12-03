using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using MinimalHello.Service;
using System;

namespace MinimalHello.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Using Microsoft's Grpc.Net
            var loggerFactory = LoggerFactory.Create(logging =>
            {
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Debug);
            });
            using var channel = GrpcChannel.ForAddress("https://localhost:5001",
                new GrpcChannelOptions { LoggerFactory = loggerFactory });
            var client = new Greeter.GreeterClient(channel);

            // Using Google's grpc-dotnet - did not work, maybe secure/insecure mismatch btw. client and server
            // Does not work - Grpc.Core.RpcException "Stream removed"
            //EnableGoogleGrpcLogging();
            //var channel = new Grpc.Core.Channel("http://localhost:5001", ChannelCredentials.Insecure);
            //var client = new Greeter.GreeterClient(channel);

            // Service call
            var helloResponse = client.GetGreeting(new HelloRequest { Name = "gRPC" });
            Console.WriteLine($"Response from GreeterService: {helloResponse.Message}");

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
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
