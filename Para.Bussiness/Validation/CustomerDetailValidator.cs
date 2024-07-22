using FluentValidation;
using Para.Data.Domain;

public class CustomerDetailValidator : AbstractValidator<CustomerDetail>
{
    public CustomerDetailValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("CustomerId must be greater than 0.");
        RuleFor(x => x.FatherName).NotEmpty().WithMessage("FatherName is required.");
        RuleFor(x => x.MotherName).NotEmpty().WithMessage("MotherName is required.");
        RuleFor(x => x.EducationStatus).NotEmpty().WithMessage("EducationStatus is required.");
        RuleFor(x => x.MontlyIncome).NotEmpty().WithMessage("MontlyIncome is required.");
        RuleFor(x => x.Occupation).NotEmpty().WithMessage("Occupation is required.");
    }
}
