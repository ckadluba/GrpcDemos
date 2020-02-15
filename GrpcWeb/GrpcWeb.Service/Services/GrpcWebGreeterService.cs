using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using static GrpcWeb.Service.GrpcWebGreeter;

namespace GrpcWeb.Service
{
    public class GrpcWebGreeterService : GrpcWebGreeterBase
    {
        private readonly ILogger<GrpcWebGreeterService> _logger;
        public GrpcWebGreeterService(ILogger<GrpcWebGreeterService> logger)
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
