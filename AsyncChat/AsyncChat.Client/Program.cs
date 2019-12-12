using AsyncChat.SharedLib.Generated;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace AsyncChat.Client
{
    class Program
    {
        private static string _userName;

        static async Task Main()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new ChatService.ChatServiceClient(channel);

            // Enter name
            Console.Write("Enter user name to join chat: ");
            _userName = Console.ReadLine();

            // Create duplex chat rpc stream
            using var chatStream = client.Chat();

            // Wire up CallbackHandler to ResponseStream - print chant messages from service
            var callbackHandler = new CallbackHandler(chatStream.ResponseStream);

            // Send init-message to join the chat
            await chatStream.RequestStream.WriteAsync(new ChatMessage { User = _userName, Text = string.Empty });

            // Run input loop
            await InputLoop(chatStream);

            // Run callback loop
            await callbackHandler.Task;
        }

        private static async Task InputLoop(Grpc.Core.AsyncDuplexStreamingCall<ChatMessage, ChatMessage> chatStream)
        {
            while (true)
            {
                var input = Console.ReadLine();
                await chatStream.RequestStream.WriteAsync(new ChatMessage { User = _userName, Text = input });
            }
        }
    }
}
