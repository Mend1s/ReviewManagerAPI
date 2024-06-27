using FluentValidation;
using ReviewManager.Application.InputModels;

namespace ReviewManager.Application.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserInputModel>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("O nome é obrigatório.")
            .MaximumLength(50)
                .WithMessage("O nome deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("O email é obrigatório.")
            .EmailAddress()
                .WithMessage("O email fornecido não é válido.");
    }
}
