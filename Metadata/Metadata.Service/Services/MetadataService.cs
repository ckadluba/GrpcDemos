using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static Metadata.Service.MetadataService;

namespace Metadata.Service.Services
{
    public class MetadataService : MetadataServiceBase
    {
        private readonly ILogger<MetadataService> _logger;
        public MetadataService(ILogger<MetadataService> logger)
        {
            _logger = logger;
        }

        public override Task<ExchangeDataRequest> ExchangeData(ExchangeDataRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"ExchangeData called");

            foreach (var item in context.RequestHeaders)
            {
                _logger.LogInformation($"ServerCallContext.RequestHeaders[] key={item.Key}, value={item.Value}");
            }

            return Task.FromResult(new ExchangeDataRequest());
        }
    }
}
