using Grpc.Core;
using MinimalGoogleGrpc.Service;
using System;

namespace MinimalGoogleGrpc.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //Using Google's grpc.core
            EnableGoogleGrpcLogging();

            var channel = new Channel("localhost:5001", ChannelCredentials.Insecure);  // Without TLS
            //var channel = new Grpc.Core.Channel("localhost:5001", new SslCredentials());  // With TLS
            var client = new Greeter.GreeterClient(channel);

            // Service call
            var helloResponse = client.GetGreeting(new HelloRequest { Name = "Leeroy Jenkins" });
            Console.WriteLine($"Response from GreeterService: {helloResponse.Message}");

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }

        private static void EnableGoogleGrpcLogging()
        {
            // Google Grpc.Core logging
            Environment.SetEnvironmentVariable("GRPC_TRACE", "api");
            Environment.SetEnvironmentVariable("GRPC_VERBOSITY", "debug");
            GrpcEnvironment.SetLogger(new Grpc.Core.Logging.ConsoleLogger());
        }
    }
}
