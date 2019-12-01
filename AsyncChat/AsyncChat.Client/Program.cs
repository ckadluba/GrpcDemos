using AsyncChat.SharedLib.Generated;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace AsyncChat.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new ChatService.ChatServiceClient(channel);

            // Create chat rpc stream
            using var chatStream = client.Chat();

            // Wire up CallbackHandler to ResponseStream - print echo from service
            var callbackHandler = new CallbackHandler(chatStream.ResponseStream);

            // Run input loop
            await InputLoop(chatStream);

            // Run callback loop
            await callbackHandler.Task;
        }

        private static async Task InputLoop(Grpc.Core.AsyncDuplexStreamingCall<ChatMessage, ChatMessage> chatStream)
        {
            Console.WriteLine("Type something");
            while (true)
            {
                var input = Console.ReadLine();
                await chatStream.RequestStream.WriteAsync(new ChatMessage { Message = input });
            }
        }
    }
}
