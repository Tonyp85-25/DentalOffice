using FluentValidation;

namespace CleanTeeth.Application.Features.DentalOffices.Commands.CreateDentalOffice;

public class CreateDentalOfficeCommandValidator: AbstractValidator<CreateDentalOfficeCommand>
{
    public CreateDentalOfficeCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("The field {PropertyName} is required.");
        RuleFor(p => p.Number).Length(5);
        RuleFor(p => p.City).NotEmpty().WithMessage("The field {PropertyName} is required.").MaximumLength(50).MinimumLength(2);
        RuleFor(p => p.Street).NotEmpty().MaximumLength(50).MinimumLength(5);
        RuleFor(p => p.Zipcode).NotEmpty().WithMessage("The field {PropertyName} is required.").Matches("/[0-9]{5}/gm");

    }
}