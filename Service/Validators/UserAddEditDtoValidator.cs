using FluentValidation;
using Service.DTOs;

namespace Service.Validators;
public class UserAddEditDtoValidator : AbstractValidator<UserAddEditDto>
{
    public UserAddEditDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required.")
            .MaximumLength(100).WithMessage("UserName must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
