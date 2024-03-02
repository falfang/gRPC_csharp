using System;
using Grpc.Core;
using Calculation;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// gRPC Server
    /// </summary>
    internal class CalculationServer : CalculationService.CalculationServiceBase
    {
        /// <summary>
        /// Callback function for receiving requests from gRPC server
        /// </summary>
        /// <param name="request">Request from the client</param>
        /// <param name="responseStream">Response stream to the server</param>
        /// <param name="context">Server call context</param>
        /// <returns></returns>
        public override async Task Operation(CalculationRequest request, IServerStreamWriter<CalculationResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("Receive request from client ! ");
            Console.WriteLine($"1st: {request.Value1}, 2nd: {request.Value2}");

            for (int i = 0; i<4; i++)
            {
                // Create response instance for sending to gRPC client
                var response = new CalculationResponse();
                switch (i)
                {
                    // Addiction
                    case 0:
                        {
                            response.Message = "Addition";
                            response.ResultValue = request.Value1 + request.Value2;
                            break;
                        }
                    // Subtraction
                    case 1:
                        {
                            response.Message = "Subtraction";
                            response.ResultValue = request.Value1 - request.Value2;
                            break;
                        }
                    // Multiplication
                    case 2:
                        {
                            response.Message = "Multiplication";
                            response.ResultValue = request.Value1 * request.Value2;
                            break;
                        }
                    // Division
                    case 3:
                        {
                            response.Message = "Division (integer)";
                            response.ResultValue = request.Value1 / request.Value2;
                            break;
                        }
                    // Other
                    default:
                        {
                            throw new InvalidOperationException("Invalid operation was designated !");
                        }
                }

                // Send response sequentially to gRPC client
                await responseStream.WriteAsync(response);

                // Wait a few second to make it look like a heavy process
                await Task.Delay(TimeSpan.FromSeconds(2));
            }

            Console.WriteLine("Server response completed !");
        }
    }
}
