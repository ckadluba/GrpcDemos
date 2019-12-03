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

# Tutorial

## Writing the Code

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

4. Edit Protobuf element to make generated source visible
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

## Troubleshooting and Configuration (to do)

### Transfer modes

Buffered, streamed, uni- or bidirectional, ...

### Service lifetime

...

### Logging

* In Google gRPC based implementations
  ```csharp
  Environment.SetEnvironmentVariable("GRPC_TRACE", "all");
  Environment.SetEnvironmentVariable("GRPC_VERBOSITY", "debug");
  Grpc.Core.GrpcEnvironment.SetLogger(new Grpc.Core.Logging.ConsoleLogger());
  ```
  https://stackoverflow.com/questions/51440399/c-sharp-grpc-client-name-resolution-failure
* In Microsoft Grpc.Net based implementations
  https://docs.microsoft.com/en-us/aspnet/core/grpc/diagnostics?view=aspnetcore-3.1

### Testing and Troubleshooting Tools

* BloomRPC - a Postman like test tool for gRPC  
  https://github.com/uw-labs/bloomrpc
* Wireshark analysis
  Only suitable for insecure HTTP/2 during development (tcp.dstport == 5001)

### Security
...

### Connection settings

...

### etc.

## References and Documentation

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