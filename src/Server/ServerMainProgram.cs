using System;
using System.Threading.Tasks;
using Calculation;
using Grpc.Core;

namespace Server
{
    /// <summary>
    /// gRPC Server Main Program
    /// </summary>
    internal class ServerMainProgram
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Server Main Program ===");
            var server = new Grpc.Core.Server
            {
                Services = { CalculationService.BindService(new CalculationServer()) },
                Ports = { new ServerPort("localhost", 12345, ServerCredentials.Insecure) }
            };

            // Start gRPC server
            server.Start();

            Console.WriteLine("CalculationServer started !");
            Console.WriteLine("Press any key to shutdown ...");
            Console.ReadKey();

            Console.WriteLine("Shutdown !");
            await server.ShutdownAsync();
        }
    }
}
