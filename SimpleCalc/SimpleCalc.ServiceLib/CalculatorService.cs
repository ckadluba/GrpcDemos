using System.Threading.Tasks;
using Grpc.Core;
using SimpleCalc.SharedLib.Generated;
using static SimpleCalc.SharedLib.Generated.CalculatorService;

namespace SimpleCalc.ServiceLib
{
    public class CalculatorService : CalculatorServiceBase
    {
        public override Task<CalculatorReply> Add(CalculatorRequest request, ServerCallContext context) =>
            Task.FromResult(new CalculatorReply { Result = request.N1 + request.N2 });

        public override Task<CalculatorReply> Divide(CalculatorRequest request, ServerCallContext context) =>
            Task.FromResult(new CalculatorReply { Result = request.N1 / request.N2 });

        public override Task<CalculatorReply> Multiply(CalculatorRequest request, ServerCallContext context) =>
            Task.FromResult(new CalculatorReply { Result = request.N1 * request.N2 });

        public override Task<CalculatorReply> Subtract(CalculatorRequest request, ServerCallContext context)
        {
            if (request.N2 == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Division by zero."));
            }

            return Task.FromResult(new CalculatorReply { Result = request.N1 - request.N2 });
        }
    }
}
