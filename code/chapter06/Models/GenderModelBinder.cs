using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace chapter06.Models
{
    public class GenderModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult != ValueProviderResult.None)
            {
                bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

                var value = valueProviderResult.FirstValue;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (Enum.TryParse<Gender>(value, out var gender))
                    {
                        bindingContext.Result = ModelBindingResult.Success(gender);
                    }
                    else
                    {
                        bindingContext.ModelState.TryAddModelError(modelName, "Invalid gender.");
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}