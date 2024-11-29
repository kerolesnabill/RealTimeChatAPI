using FluentValidation;

namespace Application.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.Name)
            .MinimumLength(1)
            .MaximumLength(50);

        RuleFor(u => u.Username)
            .MinimumLength(3)
            .MaximumLength(20)
            .Matches("^[a-zA-Z]+$")
            .WithMessage("Username must contains only alphabetic characters");

        RuleFor(u => u.Password)
            .MinimumLength(6)
            .MaximumLength(255);
    }
}
