using Backend.Core.Models.DTOs.Request;
using FluentValidation;

namespace Backend.Core.Extensions.Validators.Response;

public class UserFilterRequestValidator : AbstractValidator<UserFilterRequest>
{
    public UserFilterRequestValidator()
    {
        RuleFor(x => x.UsernameContains)
            .MaximumLength(50).When(x => x.UsernameContains != null);

        RuleFor(x => x.EmailContains)
            .MaximumLength(255).When(x => x.EmailContains != null);
    }
}