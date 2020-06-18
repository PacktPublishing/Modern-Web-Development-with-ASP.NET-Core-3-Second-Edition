using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Linq;

namespace chapter06
{
    public sealed class IsEvenClientModelValidator : IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = true.ToString().ToLowerInvariant();
            context.Attributes["data-val-iseven"] = this.GetErrorMessage(context);
        }

        private string GetErrorMessage(ClientModelValidationContext context)
        {
            var attr = context
                .ModelMetadata
                .ValidatorMetadata
                .OfType<IsEvenAttribute>()
                .SingleOrDefault();

            var msg = attr.FormatErrorMessage(context.ModelMetadata.PropertyName);

            return msg;
        }
    }
}