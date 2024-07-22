using FluentValidation;
using Para.Data.Domain;

public class CustomerAddressValidator : AbstractValidator<CustomerAddress>
{
    public CustomerAddressValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("CustomerId must be greater than 0.");
        RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required.");
        RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");
        RuleFor(x => x.AddressLine).NotEmpty().WithMessage("AddressLine is required.");
        RuleFor(x => x.ZipCode).NotEmpty().WithMessage("ZipCode is required.");
        RuleFor(x => x.IsDefault).NotNull().WithMessage("IsDefault is required.");
    }
}
