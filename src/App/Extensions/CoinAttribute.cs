using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace App.Extensions
{
    // Define o comportamento no server atravez de data-anotation
    public class CoinAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var coin = Convert.ToDecimal(value, new CultureInfo("pt-BR")); 
            }
            catch (Exception)
            {
                return new ValidationResult("Moeda em formato inválido");
            }
            return ValidationResult.Success;
        }
    }

    // O adapter define o comportamento no client
    public class CoinAttributeAdapter : AttributeAdapterBase<CoinAttribute>
    {
        public CoinAttributeAdapter(CoinAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer) { }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-coin", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return "Moeda em formato inválido";
        }
    }
    
    // Para que o adapter funcione é necessário implementar um provider
    public class CoinValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is CoinAttribute coinAttribute) return new CoinAttributeAdapter(coinAttribute, stringLocalizer);
            return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}