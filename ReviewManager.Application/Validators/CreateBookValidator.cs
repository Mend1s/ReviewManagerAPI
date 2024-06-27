using FluentValidation;
using Microsoft.AspNetCore.Http;
using ReviewManager.Application.InputModels;

namespace ReviewManager.Application.Validators
{
    public class CreateBookValidator : AbstractValidator<CreateBookInputModel>
    {
        public CreateBookValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                    .WithMessage("O título do livro é obrigatório.");

            RuleFor(x => x.Description)
                .NotEmpty()
                    .WithMessage("A descrição do livro é obrigatória.");

            RuleFor(x => x.ISBN)
                .NotEmpty()
                    .WithMessage("O ISBN do livro é obrigatório.");

            RuleFor(x => x.Author)
                .NotEmpty()
                    .WithMessage("O nome do autor é obrigatório.");

            RuleFor(x => x.Publisher)
                .NotEmpty()
                    .WithMessage("O nome da editora é obrigatório.");
            
            RuleFor(x => x.Genre)
                .NotEmpty()
                    .WithMessage("O gênero do livro é obrigatório.");
            
            RuleFor(x => x.YearOfPublication)
                .NotEmpty()
                    .WithMessage("O ano de publicação é obrigatório.")
                .GreaterThanOrEqualTo(1900)
                    .WithMessage("O ano de publicação deve ser maior ou igual a 1900.");
            
            RuleFor(x => x.NumberOfPages)
                .NotEmpty()
                    .WithMessage("O número de páginas é obrigatório.")
                .GreaterThan(0)
                    .WithMessage("O número de páginas deve ser maior que zero.");

            RuleFor(x => x.ImageUrl)
                .Must(BeAValidImage)
                    .WithMessage("A imagem deve ser PNG ou JPEG.");
        }

        private bool BeAValidImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return true;

            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(fileExtension);
        }
    }

}
