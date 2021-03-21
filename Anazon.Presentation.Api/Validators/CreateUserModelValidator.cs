using Anazon.Domain.Models.Create;
using FluentValidation;

namespace Anazon.Presentation.Api.Validators
{
    public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nome inválido");
            RuleFor(x => x.CPF).NotEmpty().WithMessage("CPF inválido").Length(11).WithMessage("Tamanho do CPF inválido");
            RuleFor(x => x.Birthdate).NotEmpty().WithMessage("Data de nascimento inválida");
            RuleFor(x => x.Sex).NotEmpty().WithMessage("Sexo inválido").Must(x => x == 'F' || x == 'M').WithMessage("Sexo inválido");
        }
    }
}
