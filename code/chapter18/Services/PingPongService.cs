using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter18.Services
{
    using Grpc.Core;
    using Microsoft.Extensions.Logging;
    using PingPong;

    public class PingPongService : PingPong.PingPongService.PingPongServiceBase
    {
        private readonly ILogger<PingPongService> _logger;

        public PingPongService(ILogger<PingPongService> logger)
        {
            this._logger = logger;
        }

        public async override Task<PongResponse> Ping(PingRequest request, ServerCallContext context)
        {
            this._logger.LogInformation("Ping received");

            return new PongResponse
            {
                Message = "Pong " + request.Name,
                Ok = Ok.Yes
            };
        }
    }
}
