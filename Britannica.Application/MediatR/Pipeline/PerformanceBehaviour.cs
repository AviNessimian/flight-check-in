using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.MediatR.Pipeline
{
    internal class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;

        public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                _timer.Start();
                var response = await next(); //Doing the actual handler since performance behavior is last on the pipeline
                _timer.Stop();

                var elapsedSeconds = _timer.ElapsedMilliseconds / 1000;

                var requestName = typeof(TRequest).Name;
                _logger.LogInformation(elapsedSeconds != 0
                    ? $"Performance: {requestName} ended within {elapsedSeconds} seconds {request}"
                    : $"Performance: {requestName} ended within {_timer.ElapsedMilliseconds} milliseconds {request}");

                return response;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
