using FluentValidation;

namespace Application.Users.Commands.UpdateUserPassword;

public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(u => u.NewPassword)
            .MinimumLength(6)
            .MaximumLength(255);
    }
}
