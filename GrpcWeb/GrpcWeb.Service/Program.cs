using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace GrpcWeb.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    ConfigureInsecureHttp2Listener(webBuilder);
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConfigureInsecureHttp2Listener(IWebHostBuilder webBuilder)
        {
            // Makes Grpc.Core client with ChannelCredentials.Insecure work 
            webBuilder.ConfigureKestrel(options =>
            {
                // This endpoint will use insecure HTTP/1 on port 5001.
                options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1;
                });
            });
        }
    }
}
