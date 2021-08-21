using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.MediatR.Pipeline
{
    internal class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (_validators.Any())
            {
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => Task.Factory.StartNew(() =>
                    {
                        var context = new ValidationContext<TRequest>(request);
                        return v.Validate(context);
                    }, cancellationToken)
                ));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).Distinct().ToList();
                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }
}
