using System.Threading.Tasks;
using AsyncChat.SharedLib.Generated;
using Grpc.Core;
using static AsyncChat.SharedLib.Generated.ChatService;

namespace AsyncChat.ServiceLib
{
    public class ChatService : ChatServiceBase
    {
        public override async Task Chat(IAsyncStreamReader<ChatMessage> requestStream, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            // TODO: Replace simple echo servive with real chatroom functionality
            await foreach (var requestMessage in requestStream.ReadAllAsync())
            {
                await responseStream.WriteAsync(
                    new ChatMessage { Message = requestMessage.Message });
            }
        }
    }
}
