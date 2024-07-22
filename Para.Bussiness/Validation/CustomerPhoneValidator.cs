using FluentValidation;
using Para.Data.Domain;

public class CustomerPhoneValidator : AbstractValidator<CustomerPhone>
{
    public CustomerPhoneValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("CustomerId must be greater than 0.");
        RuleFor(x => x.CountyCode).NotEmpty().WithMessage("CountyCode is required.");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required.");
        RuleFor(x => x.IsDefault).NotNull().WithMessage("IsDefault is required.");
    }
}
