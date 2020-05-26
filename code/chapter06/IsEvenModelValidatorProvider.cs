using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter06
{
    public sealed class IsEvenModelValidatorProvider : IModelValidatorProvider
    {
        public void CreateValidators(ModelValidatorProviderContext context)
        {
            if (context.ModelMetadata.ModelType == typeof(string)
                || context.ModelMetadata.ModelType == typeof(int)
                || context.ModelMetadata.ModelType == typeof(uint)
                || context.ModelMetadata.ModelType == typeof(long)
                || context.ModelMetadata.ModelType == typeof(ulong)
                || context.ModelMetadata.ModelType == typeof(short)
                || context.ModelMetadata.ModelType == typeof(ushort)
                || context.ModelMetadata.ModelType == typeof(float)
                || context.ModelMetadata.ModelType == typeof(double))
            {
                if (!context.Results.Any(x => x.Validator is IsEvenModelValidator))
                {
                    context.Results.Add(new ValidatorItem
                    {
                        Validator = new IsEvenModelValidator(),
                        IsReusable = true
                    });
                }
            }
        }
    }
}
