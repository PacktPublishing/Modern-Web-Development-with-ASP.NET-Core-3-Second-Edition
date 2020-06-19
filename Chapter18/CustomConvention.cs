using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace chapter18
{
    internal class CustomConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            application.Filters.Add(new CustomFilter());
        }
    }
}