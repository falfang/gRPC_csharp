using System;
using System.Threading;
using System.Threading.Tasks;

using Calculation;
using Grpc.Core;

namespace Client
{
    /// <summary>
    /// gRPC Client Main Program
    /// </summary>
    internal class ClientMainProgram
    {
        /// <summary>
        /// Whether to cancel receiving response from the server
        /// </summary>
        private static bool _cancelService = true;


        /// <summary>
        /// Main Program for gRPC Client
        /// </summary>
        /// <param name="args">Arguments</param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Client Main Program ===");
            var channel = new Channel("localhost", 12345, ChannelCredentials.Insecure);
            var client = new CalculationService.CalculationServiceClient(channel);

            var request = new CalculationRequest();

            // Set the values of request
            Console.Write("Enter 1st value: ");
            request.Value1 = int.Parse(Console.ReadLine());

            Console.Write("Enter 2nd value: ");
            request.Value2 = int.Parse(Console.ReadLine());

            // Create CancellationTokenSource to pass to server
            var cancelToken = new CancellationTokenSource();

            // Send reply with CancellationToken
            var reply = client.Operation(request, Metadata.Empty, null, cancelToken.Token);

            // Cancel service after 3 seconds from sending request
            if (_cancelService)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(3000);
                    cancelToken.Cancel();
                });
            }
            

            try
            {
                while (await reply.ResponseStream.MoveNext())
                {
                    Console.WriteLine($"{reply.ResponseStream.Current.Message}: {reply.ResponseStream.Current.ResultValue}");
                }
            }
            catch (Grpc.Core.RpcException ex) when (ex.Status.StatusCode == StatusCode.Cancelled)
            {
                Console.Write($"Server cancelled !");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
