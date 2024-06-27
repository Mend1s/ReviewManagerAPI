using FluentValidation;
using ReviewManager.Application.InputModels;

namespace ReviewManager.Application.Validators
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewInputModel>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.Note)
                .InclusiveBetween(1, 5)
                    .WithMessage("A nota deve estar entre 1 e 5.");

            RuleFor(x => x.Description)
                .NotEmpty()
                    .WithMessage("A descrição é obrigatória.")
                .MaximumLength(500)
                    .WithMessage("A descrição deve ter no máximo 500 caracteres.");

            RuleFor(x => x.IdUser)
                .GreaterThan(0)
                    .WithMessage("O id do usuário é obrigatório.");

            RuleFor(x => x.IdBook)
                .GreaterThan(0)
                    .WithMessage("O id do livro é obrigatório.");
        }
    }
}
