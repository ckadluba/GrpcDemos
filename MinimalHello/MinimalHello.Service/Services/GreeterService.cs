using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace MinimalHello.Service
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> GetGreeting(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
                {
                    Message = "Hello " + request.Name
                });
        }
    }
}
