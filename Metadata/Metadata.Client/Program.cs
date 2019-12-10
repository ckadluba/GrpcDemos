using Grpc.Core;
using Grpc.Net.Client;
using Metadata.Service;
using System;
using System.Threading.Tasks;

namespace Metadata.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new MetadataService.MetadataServiceClient(channel);

            // First call with per-call metadata
            var callOptions = new CallOptions(
                new Grpc.Core.Metadata {
                    new Grpc.Core.Metadata.Entry("Call1RequestField1", "Call1RequestValue1"),
                    new Grpc.Core.Metadata.Entry("Call1RequestField2", "Call1RequestValue2")
              });
            client.ExchangeData(new ExchangeDataRequest(), callOptions);

            Console.WriteLine($"Call1 finished");
            Console.WriteLine("Press any key ...");
            Console.ReadKey();

            // Second call with per-call metadata
            var callOptions2 = new CallOptions(
                new Grpc.Core.Metadata {
                    new Grpc.Core.Metadata.Entry("Call2RequestField1", "Call2RequestValue1"),
                    new Grpc.Core.Metadata.Entry("Call2RequestField2", "Call2RequestValue2")
              });
            var call2 = client.ExchangeDataAsync(new ExchangeDataRequest(), callOptions2);

            // Proccess resonse metadata in second call from server
            await call2;
            Console.WriteLine($"Call2 finished");
            var metadata = call2.GetTrailers();
            foreach (var item in metadata)
            {
                Console.WriteLine($"GetTrailers()[] key={item.Key}, value={item.Value}");
            }

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }
    }
}
