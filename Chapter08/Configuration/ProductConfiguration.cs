using chapter08.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;

namespace chapter08.Configuration
{
    public class ProductConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            var products = builder.EntitySet<Product>("Products").EntityType.HasKey(p => p.Id);
        }
    }
}
