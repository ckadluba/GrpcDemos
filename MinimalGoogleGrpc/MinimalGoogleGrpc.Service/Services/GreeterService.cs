using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace MinimalGoogleGrpc.Service
{
    public class GreeterService : Greeter.GreeterBase
    {
        public override Task<HelloReply> GetGreeting(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
