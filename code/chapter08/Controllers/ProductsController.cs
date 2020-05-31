using chapter08.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace chapter08.Controllers
{
    public class ProductsController : ODataController
    {
        private static Product[] _products = new[] { new Product { Id = 1, Name = "A" }, new Product { Id = 2, Name = "B" } };


        [EnableQuery]
        public IQueryable<Product> Get() => _products.AsQueryable();
    }
}
