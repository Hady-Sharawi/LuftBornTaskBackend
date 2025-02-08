using FluentValidation;
using Service.DTOs;

namespace Service.Validators;
public class ProductAddEditDtoValidator : AbstractValidator<ProductAddEditDto>
{
    public ProductAddEditDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("ProductName is required.")
            .MaximumLength(100).WithMessage("ProductName must not exceed 100 characters.");
    }
}
