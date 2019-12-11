using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using static Deadline.Service.DelayedGreeter;

namespace Deadline.Service
{
    public class DelayedGreeterService : DelayedGreeterBase
    {
        private readonly ILogger<DelayedGreeterService> _logger;
        public DelayedGreeterService(ILogger<DelayedGreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<DelayedHelloReply> GetDelayedGreeting(DelayedHelloRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"DelayedHelloRequest, delaying {request.Delay}ms");

            Task.WaitAll(new Task[] { Task.Delay(request.Delay) }, context.CancellationToken);

            _logger.LogInformation("Sending DelayedHelloReply");

            return Task.FromResult(new DelayedHelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
