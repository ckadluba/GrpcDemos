using AsyncChat.SharedLib.Generated;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using static AsyncChat.SharedLib.Generated.ChatService;

namespace AsyncChat.ServiceLib
{
    public class ChatService : ChatServiceBase
    {
        private readonly ChatHub _chatHub;

        public ChatService(ChatHub chatHub)
        {
            _chatHub = chatHub ?? throw new ArgumentNullException(nameof(chatHub));
        }

        public override async Task Chat(IAsyncStreamReader<ChatMessage> requestStream, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            await foreach (var requestMessage in requestStream.ReadAllAsync())
            {
                await _chatHub.HandleIncomingMessage(requestMessage, responseStream);
            }
        }
    }
}
