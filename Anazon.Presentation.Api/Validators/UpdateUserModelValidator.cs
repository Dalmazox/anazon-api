using Anazon.Domain.Models.Update;
using FluentValidation;

namespace Anazon.Presentation.Api.Validators
{
    public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
    {
        public UpdateUserModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nome inválido");
            RuleFor(x => x.Birthdate).NotEmpty().WithMessage("Data de nascimento inválida");
            RuleFor(x => x.Sex).NotEmpty().WithMessage("Sexo inválido").Must(x => x == 'F' || x == 'M').WithMessage("Sexo inválido");
        }
    }
}
