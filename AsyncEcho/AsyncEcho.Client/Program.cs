using AsyncEcho.SharedLib.Generated;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace AsyncEcho.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new EchoService.EchoServiceClient(channel);

            // Create chat rpc stream
            using var echoStream = client.Echo();

            // Wire up CallbackHandler to ResponseStream - print echo from service
            var callbackHandler = new CallbackHandler(echoStream.ResponseStream);

            // Run input loop
            await InputLoop(echoStream);

            // Run callback loop
            await callbackHandler.Task;
        }

        private static async Task InputLoop(Grpc.Core.AsyncDuplexStreamingCall<EchoMessage, EchoMessage> echoStream)
        {
            Console.WriteLine("Type something");
            while (true)
            {
                var input = Console.ReadLine();
                await echoStream.RequestStream.WriteAsync(new EchoMessage { Message = input });
            }
        }
    }
}
