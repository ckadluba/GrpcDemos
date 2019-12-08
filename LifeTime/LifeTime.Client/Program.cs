using Grpc.Core;
using Grpc.Net.Client;
using LifeTime.SharedLib;
using System;
using static LifeTime.SharedLib.LifeTimeService;

namespace LifeTime.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Call service on channel for :5001 - new instance per request
            using var channel5001 = GrpcChannel.ForAddress("https://localhost:5001");
            CallServiceTwice(channel5001);

            // Call service on channel for 5002: - single instance used for all requests
            using var channel5002 = GrpcChannel.ForAddress("https://localhost:5002");
            CallServiceTwice(channel5002);

            // Call service on channel for 5003: - Google gRPC. Also uses single instance used for all requests.
            var channel5003 = new Channel("localhost:5003", ChannelCredentials.Insecure);
            CallServiceTwice(channel5003);

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }

        private static void CallServiceTwice(ChannelBase channel)
        {
            var client = new LifeTimeServiceClient(channel);

            var getInstanceResponse = client.GetInstance(new GetInstanceRequest());
            Console.WriteLine($"GetInstance #1 from {channel.Target}: {getInstanceResponse.Message}");
            var getInstanceResponse2 = client.GetInstance(new GetInstanceRequest());
            Console.WriteLine($"GetInstance #2 from {channel.Target}: {getInstanceResponse2.Message}");
        }
    }
}
