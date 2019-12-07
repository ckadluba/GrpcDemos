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

            // Service call
            var helloResponse = client.GetGreeting(new HelloRequest { Name = "Leeory Jenkins" });
            Console.WriteLine($"Response from GreeterService: {helloResponse.Message}");

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }
    }
}
