using Grpc.Core;
using LifeTime.SharedLib;
using System;
using System.Threading.Tasks;
using static LifeTime.SharedLib.LifeTimeService;

namespace LifeTime.ServiceLib
{
    public class LifeTimeService : LifeTimeServiceBase
    {
        private readonly Guid _instanceGuid = Guid.NewGuid();

        public override Task<GetInstanceReply> GetInstance(GetInstanceRequest request, ServerCallContext context)
        {
            return Task.FromResult(
                new GetInstanceReply
                {
                    Message = $"Instance={_instanceGuid}"
                });
        }
    }
}
