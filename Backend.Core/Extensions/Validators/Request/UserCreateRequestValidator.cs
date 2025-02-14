using Backend.Core.Models.DTOs.Request;
using FluentValidation;

namespace Backend.Core.Extensions.Validators.Request;

public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
{
    public UserCreateRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Имя пользователя обязательно")
            .Length(3, 50).WithMessage("Длина имени от 3 до 50 символов")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Допустимы буквы, цифры и подчёркивание");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен")
            .MinimumLength(8).WithMessage("Минимум 8 символов")
            .MaximumLength(100).WithMessage("Максимум 100 символов");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Некорректный формат email")
            .MaximumLength(255).WithMessage("Максимум 255 символов");

        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Отдел обязателен")
            .NotEqual(Guid.Empty).WithMessage("Некорректный идентификатор отдела");
    }
}