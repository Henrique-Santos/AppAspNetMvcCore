using Business.Models.Validations.Documents;
using FluentValidation;

namespace Business.Models.Validations
{
    public class SupplierValidation : AbstractValidator<Supplier>
    {
        public SupplierValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 100)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            When(x => x.SupplierType == SupplierType.NaturalPerson, () => 
            {
                RuleFor(x => x.Document.Length)
                    .Equal(ZipCodeValidation.ZipCodeLength)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

                RuleFor(x => ZipCodeValidation.Validate(x.Document))
                    .Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });

            When(x => x.SupplierType == SupplierType.JuridicalPersion, () => 
            {
                RuleFor(x => x.Document.Length)
                       .Equal(CnpjValidation.CnpjLength)
                       .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

                RuleFor(x => CnpjValidation.Validate(x.Document))
                    .Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });
        }
    }
}