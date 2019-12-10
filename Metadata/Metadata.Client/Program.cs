using Grpc.Core;
using Grpc.Net.Client;
using Metadata.Service;
using System;

namespace Metadata.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO define AsyncAuthInterceptor/CallCredentials with metadata for each request

            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Service.MetadataService.MetadataServiceClient(channel);

            // Service call with metadata
            var callOptions = new CallOptions(
                new Grpc.Core.Metadata {
                    new Grpc.Core.Metadata.Entry("Field1", "Value1"),
                    new Grpc.Core.Metadata.Entry("Field2", "Value2")
              });
            var response = client.ExchangeData(new ExchangeDataRequest(), callOptions);

            // TODO send second call with different metadata

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }
    }
}
