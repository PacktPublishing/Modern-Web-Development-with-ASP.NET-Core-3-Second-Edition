using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter06
{
    public sealed class IsEvenModelValidator : IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            if (context.Model != null)
            {
                try
                {
                    var value = Convert.ToDouble(context.Model);
                    if ((value % 2) == 0)
                    {
                        yield break;
                    }
                }
                catch { }
            }

            yield return new ModelValidationResult(
                context.ModelMetadata.PropertyName,
                $"{context.ModelMetadata.PropertyName} is not even.");
        }
    }
}
