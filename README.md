# gRPC Sample Programs

## MinimalHello

The simplest possible client and server gRPC sample app consisting of only two projects.  
For demo purposes, it uses insecure transport over HTTP/2 without TLS. Demonstrates also Microsoft Grpc.Net and Google Grpc.Core logging.

## SimpleCalc

A simple calculator service with a client using synchronous gRPC calls with ASP .Net Core 3.0.  

Execute SimpleCalc.ServiceHost and SimpleCalc.Client.

## AsyncEcho

A service that just echos the input from the client using an asynchronous gRPC call.

Execute AsyncEcho.ServiceHost and AsyncEcho.Client.

## AsyncChat (to do)

A small chat server with a console client using asynchronous gRPC calls.

Execute AsyncChat.ServiceHost and two or more instances of AsyncChat.Client.

# Writing the Code

This example is based on the SimpleCalc sample app.

1. Maybe you need to install dotnet-grpc  
   ```
   dotnet tool install -g dotnet-grpc
   ```

1. Create SimpleCalc.SharedLib as netcoreapp3.0 classlib  
   netstandard2.x would be desirable but does not work because of tooling issue 
   https://github.com/aspnet/AspNetCore.Docs/issues/14702

2. Create service contract  
   In SharedLib create SimpleCalc.proto  

3. Generate client and server stubs  
   In SharedLib: Add > Service Reference > gRPC > File: .proto file, Type: Client and Server

4. Edit Protobuf element to make generated source visible in Solution Explorer
   ```xml
   <Protobuf Include="calculator.proto"
             OutputDir="%(RelativePath)"
             CompileOutputs="false" />
   ```
   Attention: don't change back or you get errors in clients (duplicated definitions).

5. Create SimpleCalc.ServiceLib as netcoreapp3.0 classlib.  
   Add project reference to SharedLib.  
   Add class CalculatorService deriving from CalculatorServiceBase.  
   Implement service methods.

6. Create SimpleCalc.ServiceHost by Add > New Project > "gRPC Service" template  
   Delete unused generated directories Services and Proto  
   Add project reference to ServiceLib.  

7. Create SimpleCalc.Client as console app  
   Add reference to SharedLib.  
   Install NuGet package Grpc.Core if using that instead of grpc-dotnet
   Create instance of CalculatorServiceClient and use with GrpcChannel.ForAddress()

# ASP.Net Core Integration

## Transfer modes

Buffered, streamed, uni- or bidirectional, ...

## Service lifetime

When is the service class created/reused?

## Security

...

## Connection settings

...

## ServiceContext

...

# Logging

## Google gRPC based Implementations
* Code (Client and Server)
  ```csharp
  Environment.SetEnvironmentVariable("GRPC_TRACE", "all");
  Environment.SetEnvironmentVariable("GRPC_VERBOSITY", "debug");
  Grpc.Core.GrpcEnvironment.SetLogger(new Grpc.Core.Logging.ConsoleLogger());
  ```
* https://stackoverflow.com/questions/51440399/c-sharp-grpc-client-name-resolution-failure

## Microsoft Grpc.Net based Implementations  
* https://docs.microsoft.com/en-us/aspnet/core/grpc/diagnostics?view=aspnetcore-3.1
* Server config
  ```json
  {
    "Logging": {
      "LogLevel": {
        "Default": "Debug",
        "System": "Debug",
        "Microsoft": "Debug",
        "Grpc": "Debug"
      }
    },
    "AllowedHosts": "*",
    "Kestrel": {
      "EndpointDefaults": {
        "Protocols": "Http2"
      }
    }
  }
  ```
* Server code
  ```csharp
  public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
          .ConfigureLogging(logging =>
          {
              logging.AddFilter("Grpc", LogLevel.Debug);
          })
          .ConfigureWebHostDefaults(webBuilder =>
          {
             webBuilder.UseStartup<Startup>();
          });
  ```
* Client code
  ```csharp
  var loggerFactory = LoggerFactory.Create(logging =>
  {
      logging.AddConsole();
      logging.SetMinimumLevel(LogLevel.Debug);
  });
  using var channel = GrpcChannel.ForAddress("https://localhost:5001",
      new GrpcChannelOptions { LoggerFactory = loggerFactory });
  var client = new Greeter.GreeterClient(channel);
  ```

# Tools

* gRPCurl - a curl for gRPC
  * https://github.com/fullstorydev/grpcurl
  * Command line tool. Many features but not easy to use.
  * Example: call MinimalHello.Service with unencrypted HTTP/2 
    ```powershell
    .\grpcurl.exe -v -plaintext -d '{ \"name\": \"Johnny\"}' -import-path "C:\source\github\GrpcDemos\MinimalHello\MinimalHello.Service\Protos\" -proto Greeter.proto localhost:5001 Greet.Greeter/GetGreeting
    ```   
    ![Grpcurl](grpcurl.png)
  * Did not work with TLS 😢
* BloomRPC - a Postman like test tool for gRPC
  * https://github.com/uw-labs/bloomrpc
  * Enter only host[:port] in address field, not full URI like "https://...". Uri is generated according to TLS setting.
  * Could add ASP.Net Core dev certificate as .crt but unfortunately could not get to work with TLS 😢 because of https://github.com/uw-labs/bloomrpc/issues/100 
* gRPCox - Web based gRPC test tool running in a Docker container  
  * https://github.com/gusaul/grpcox 
  * Requires server to support gRPC reflection 😢
* Postman + grpc-json-proxy
  * https://github.com/jnewmano/grpc-json-proxy and https://medium.com/@jnewmano/grpc-postman-173b62a64341
  * Project seems to be dead on GitHub 😢
  * TLS not supported 😢
* Wireshark
  * Only suitable for general connectivity problems or with insecure HTTP/2 during development
  * TLS also possible but requires cumbersome server cert + private key setup in Wireshark
  * Trace packets on loopback interface with display filter "tcp.port == 5001".

# References and Documentation

* Official gRPC site and GitHub  
  https://grpc.io/
* gRPC on GitHub  
  https://github.com/grpc/grpc
* gRPC wire format  
  https://github.com/grpc/grpc/blob/master/doc/PROTOCOL-HTTP2.md
* A curated list of useful gRPC resources  
  https://github.com/grpc-ecosystem/awesome-grpc
* Grpc.Net client "stream removed" problem  
  https://stackoverflow.com/questions/55747287/unable-to-make-a-connection-between-trivial-c-sharp-grpc-client-and-server?noredirect=1