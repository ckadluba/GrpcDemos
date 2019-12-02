# gRPC Sample Programs

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

## Writing the code

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

## Troubleshooting and configuration

TODO.
* Transfer modes (buffered, streamed, uni- or bidirectional)
* Service lifetime
* Callbacks
* Logging
* Testing tools (like Postman)
* Security
* Connection settings
* etc.
