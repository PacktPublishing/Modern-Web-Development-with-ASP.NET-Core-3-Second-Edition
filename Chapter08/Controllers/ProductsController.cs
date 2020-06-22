using chapter08.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace chapter08.Controllers
{
    /// <summary>
    /// Controller that deals with products.
    /// </summary>
    [ApiVersion("1.0")]
    //This next line can be removed, it is here just to illustrate the usage of ODataRoutePrefix with a version
    [ODataRoutePrefix("{version:apiVersion}/Products")]
    public class ProductsController : ODataController
    {
        private static Product[] _products = new[] { new Product { Id = 1, Name = "A", Price = 10.5M }, new Product { Id = 2, Name = "B", Price = 20M } };

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>All available products.</returns>
        /// <response code="200">Products successfully retrieved.</response>
        [EnableQuery]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ODataValue<IEnumerable<Product>>), StatusCodes.Status200OK)]
        [HttpGet]
        public IQueryable<Product> Get()
        {
            return _products.AsQueryable();
        }
    }
}
