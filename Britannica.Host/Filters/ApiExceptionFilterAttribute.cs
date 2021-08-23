using Britannica.Application.Exceptions;
using Britannica.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static Britannica.Host.Filters.AppValidationProblemDetails;

namespace Britannica.Host.Filters
{
    public partial class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {

        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute()
        {
            // Register known exception types and handlers.
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(BusinessRuleException), HandleAppBusinessRuleException },
                { typeof(DbUpdateConcurrencyException), HandleDbUpdateConcurrencyException },

            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;
            var errors = exception.Errors.Select(x => new ValidationError { PropertyName = x.PropertyName, ErrorMessage = x.ErrorMessage }).Distinct().ToList();
            var details = new AppValidationProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Message = exception.Message,
                Errors = errors
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };

            context.ExceptionHandled = true;
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {
            var errors = context.ModelState.Select(x => new ValidationError { PropertyName = x.Key, ErrorMessage = x.Value.RawValue.ToString() }).Distinct().ToList();
            var details = new AppValidationProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Message = context.Exception.Message,
                Errors = errors
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;
            var details = new AppNotFoundProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Message = context.Exception.Message,
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status404NotFound
            };

            context.ExceptionHandled = true;
        }

        private void HandleAppBusinessRuleException(ExceptionContext context)
        {
            var exception = context.Exception as BusinessRuleException;
            var details = new BusinessRuleProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Message = context.Exception.Message,
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };

            context.ExceptionHandled = true;
        }

        private void HandleDbUpdateConcurrencyException(ExceptionContext context)
        {
            var exception = context.Exception as DbUpdateConcurrencyException;
            var details = new UpdateConcurrencyProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Message = context.Exception.Message,
            };

            var exceptionEntry = exception.Entries.Single();
            var databaseEntry = exceptionEntry.GetDatabaseValues();
            if (databaseEntry == null)
            {
                details.Description = "Unable to save changes. The Seat was deleted by another user.";
            }
            else
            {
                details.EntityType = databaseEntry.EntityType.DisplayName();
                details.Description = $"The record you attempted to edit " +
                    $"was modified by another user after you got the original value. The " +
                   $"edit operation was canceled and the current values in the database " +
                   $"have been displayed. If you still want to edit this record, click " +
                   $"the Save button again. Otherwise click the Back to List hyperlink.";
            }

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };

            context.ExceptionHandled = true;
        }



        private void HandleUnknownException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
