using AsyncChat.SharedLib.Generated;
using Grpc.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncChat.Client
{
    internal class CallbackHandler
    {
        private readonly IAsyncStreamReader<ChatMessage> _responseStream;
        private readonly CancellationToken _cancellationToken;

        public CallbackHandler(IAsyncStreamReader<ChatMessage> responseStream)
          : this(responseStream, CancellationToken.None)
        {
        }

        public CallbackHandler(IAsyncStreamReader<ChatMessage> responseStream,
          CancellationToken cancellationToken)
        {
            _responseStream = responseStream ?? throw new ArgumentNullException(nameof(responseStream));
            _cancellationToken = cancellationToken;
            Task = Task.Run(Consume, _cancellationToken);
        }

        public Task Task { get; }

        async Task Consume()
        {
            await foreach (var responseMessage in _responseStream.ReadAllAsync())
            {
                Console.WriteLine($"{responseMessage.User}: {responseMessage.Text}");
            }
        }
    }
}
