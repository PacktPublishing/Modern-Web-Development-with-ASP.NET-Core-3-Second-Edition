using chapter08.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace chapter08.Controllers
{
    /// <summary>
    /// Controller that deals with orders.
    /// </summary>
    [ApiVersion("1.0")]
    //[ODataRoutePrefix("Orders")]
    public class OrdersController : ODataController
    {
        private static Order[] _orders = new[]
        {
            new Order { Id = 1, Timestamp = DateTime.Today, Products = new List<Product> { new Product { Id = 1, Name = "A", Price = 10.5M } } }
        };

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>All available orders.</returns>
        /// <response code="200">Orders successfully retrieved.</response>
        [EnableQuery]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ODataValue<IEnumerable<Order>>), StatusCodes.Status200OK)]
        [HttpGet]
        public IQueryable<Order> Get()
        {
            return _orders.AsQueryable();
        }
    }
}
