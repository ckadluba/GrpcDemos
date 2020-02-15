using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcWeb.Service;
using System;
using System.Net.Http;
using static GrpcWeb.Service.GrpcWebGreeter;

namespace GrpcWeb.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure a channel to use gRPC-Web
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new Version(1, 1), new HttpClientHandler());

            // Force insecure HTTP/1 for sending request to demonstrate gRPC-Web
            using var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler)
            });
            var client = new GrpcWebGreeterClient(channel);

            // Service call
            var helloResponse = client.GetGreeting(new HelloRequest { Name = "Leeory Jenkins over gRPC-Web" });
            Console.WriteLine($"Response from GrpcWebGreeterService: {helloResponse.Message}");

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }
    }
}
