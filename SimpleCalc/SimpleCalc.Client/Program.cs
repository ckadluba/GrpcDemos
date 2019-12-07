using Grpc.Net.Client;
using SimpleCalc.SharedLib.Generated;
using System;
using System.Threading.Tasks;

namespace SimpleCalc.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Using Microsoft's Grpc
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new CalculatorService.CalculatorServiceClient(channel);

            const int n1 = 34;
            const int n2 = 76;
            var sum = client.Add(new CalculatorRequest { N1 = n1, N2 = n2 });
            Console.WriteLine($"Called service: {n1} + {n2} = {sum.Result}");

            var difference = client.Subtract(new CalculatorRequest { N1 = n1, N2 = n2 });
            Console.WriteLine($"Called service: {n1} - {n2} = {difference.Result}");

            var product = client.Multiply(new CalculatorRequest { N1 = n1, N2 = n2 });
            Console.WriteLine($"Called service: {n1} * {n2} = {product.Result}");

            var division = client.Divide(new CalculatorRequest { N1 = n1, N2 = n2 });
            Console.WriteLine($"Called service: {n1} / {n2} = {division.Result}");

            var divisionAsync = await client.DivideAsync(new CalculatorRequest { N1 = n1, N2 = n2 });
            Console.WriteLine($"Called service async: {n1} / {n2} = {divisionAsync.Result}");

            var divisionError = client.Divide(new CalculatorRequest { N1 = n1, N2 = 0 });
            Console.WriteLine($"Called service with error: {n1} / {0} = {divisionError.Result}");
        }
    }
}
