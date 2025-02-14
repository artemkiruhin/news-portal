using Backend.Core.Models.DTOs.Request;
using FluentValidation;

namespace Backend.Core.Extensions.Validators.Response;

public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
{
    public UserUpdateRequestValidator()
    {
        RuleFor(x => x.Username)
            .Length(3, 50).When(x => x.Username != null)
            .Matches("^[a-zA-Z0-9_]+$").When(x => x.Username != null);

        RuleFor(x => x.NewPassword)
            .MinimumLength(8).When(x => !string.IsNullOrEmpty(x.NewPassword))
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .MaximumLength(255);
    }
}