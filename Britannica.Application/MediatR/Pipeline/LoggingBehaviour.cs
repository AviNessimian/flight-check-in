using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.MediatR.Pipeline
{
    internal class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            try
            {
                _logger.LogDebug($"Handling {requestName}");
                //LogRequestProps(request);
                var output = await next();
                _logger.LogDebug($"{requestName} execution success");
                return output;
            }
            catch (ValidationException ex)
            {
                if (!ex.Errors.Any())
                {
                    _logger.LogInformation(ex.Message);
                }

                foreach (var failure in ex.Errors.Select(c => new { c.PropertyName, c.ErrorMessage }).Distinct().ToList())
                {
                    _logger.LogInformation("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                }
                throw;
            }
        }

        private void LogRequestProps(TRequest request)
        {
            Type myType = request.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(request, null);
                _logger.LogDebug($"{prop.Name} : {propValue}");
            }
        }
    }
}
