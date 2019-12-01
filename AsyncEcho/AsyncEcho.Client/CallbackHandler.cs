using AsyncEcho.SharedLib.Generated;
using Grpc.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncEcho.Client
{
    internal class CallbackHandler
    {
        private readonly IAsyncStreamReader<EchoMessage> _responseStream;
        private readonly CancellationToken _cancellationToken;

        public CallbackHandler(IAsyncStreamReader<EchoMessage> responseStream)
          : this(responseStream, CancellationToken.None)
        {
        }

        public CallbackHandler(IAsyncStreamReader<EchoMessage> responseStream,
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
                Console.WriteLine($"Echo: {responseMessage.Message}");
            }
        }
    }
}
