using Britannica.Application.Interactors;
using FluentValidation;
using FluentValidation.Results;

namespace Britannica.Application.Validation
{
    public class CheckInValidation : AbstractValidator<CheckInRequest>
    {
        public override ValidationResult Validate(ValidationContext<CheckInRequest> context)
        {
            RuleFor(request => request.FlightId).NotNull().NotEmpty();
            RuleFor(request => request.PassengerId).NotNull().NotEmpty();
            RuleFor(request => request.SeatId).NotNull().NotEmpty();
            RuleFor(request => request.Baggages).NotNull().NotEmpty();
            RuleForEach(x => x.Baggages).NotNull().NotEmpty();
            RuleForEach(x => x.Baggages).ChildRules(orders => {
                orders.RuleFor(x => x.Weight).GreaterThan(0);
            });

            return base.Validate(context);
        }
    }
}
