using System.Threading.Tasks;
using AsyncEcho.SharedLib.Generated;
using Grpc.Core;
using static AsyncEcho.SharedLib.Generated.EchoService;

namespace AsyncEcho.ServiceLib
{
    public class EchoService : EchoServiceBase
    {
        public override async Task Echo(IAsyncStreamReader<EchoMessage> requestStream, IServerStreamWriter<EchoMessage> responseStream, ServerCallContext context)
        {
            await foreach (var requestMessage in requestStream.ReadAllAsync())
            {
                await responseStream.WriteAsync(
                    new EchoMessage { Message = requestMessage.Message });
            }
        }
    }
}
