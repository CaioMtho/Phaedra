using FluentValidation;
using Phaedra.Server.DTO.User;

namespace Phaedra.Server.Validations.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Empty Email")
                .EmailAddress().WithMessage("Invalid Email");
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username empty")
                .Length(4, 50).WithMessage("Name must be between 4 and 50 characters");
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password empty")
                .Length(6, 255).WithMessage("Password must be between 6 and 255 characters");
        }
    }
}
