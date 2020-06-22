using chapter08.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;

namespace chapter08.Configuration
{
    public class OrderConfiguration : IModelConfiguration
    {
        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            var orders = builder.EntitySet<Order>("Orders").EntityType.HasKey(p => p.Id);
        }
    }
}
