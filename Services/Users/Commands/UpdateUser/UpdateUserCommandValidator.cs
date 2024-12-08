using FluentValidation;

namespace RealTimeChatAPI.Services.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .MaximumLength(50)
            .When(u => u.Name != null);

        RuleFor(u => u.Username)
            .MinimumLength(3)
            .MaximumLength(20)
            .Matches("^[a-zA-Z]+[0-9]*$")
            .WithMessage("Username must contain only alphabetic characters followed by optional numbers")
            .When(u => u.Username != null);
    }
}
