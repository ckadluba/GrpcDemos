using Deadline.Service;
using Grpc.Core;
using Grpc.Net.Client;
using System;

namespace Deadline.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new DelayedGreeter.DelayedGreeterClient(channel);

            while (true)
            {
                var clientTimeout = ConsoleReadIntegerValue("Enter client timeout in ms: ");
                var serverDelay = ConsoleReadIntegerValue("Enter server delay in ms: ");

                try
                {
                    // Calculate deadline based on UtcNow
                    var deadline = DateTime.UtcNow.AddMilliseconds(clientTimeout);

                    Console.WriteLine($"Sending request (current time: {DateTime.UtcNow}), deadline: {deadline}, server delay: {serverDelay}");

                    var reply = client.GetDelayedGreeting(
                        new DelayedHelloRequest { Name = "Joe", Delay = serverDelay },
                        deadline: deadline);

                    Console.WriteLine($"Received response (current time: {DateTime.UtcNow}): {reply.Message}");
                }
                catch (RpcException rpcException)
                {
                    // If Deadline hit: rpcException.StatusCode == Grpc.Core.StatusCode.DeadlineExceeded
                    Console.WriteLine($"Client caught RpcException {rpcException}");
                }
            }
        }

        private static int ConsoleReadIntegerValue(string message)
        {
            Console.Write(message);
            var clientTimeoutString = Console.ReadLine();
            var clientTimeout = Int32.Parse(clientTimeoutString);
            return clientTimeout;
        }
    }
}
