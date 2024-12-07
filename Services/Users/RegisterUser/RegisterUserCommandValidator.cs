using FluentValidation;

namespace RealTimeChatAPI.Services.Users.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(u => u.Username)
            .MinimumLength(3)
            .MaximumLength(20)
            .Matches("^[a-zA-Z]+[0-9]*$")
            .WithMessage("Username must contain only alphabetic characters followed by optional numbers");

        RuleFor(u => u.Password)
            .MinimumLength(6)
            .MaximumLength(255);
    }
}
