using chapter08.Controllers.Controllers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chapter08.Controllers
{
    namespace Controllers.V1
    {
        [ApiController]
        public class ApiController : ControllerBase
        {
            //http://localhost:5000/api
            //http://localhost:5000/api?api-version=1.0
            //http://localhost:5000/api/json => default json
            //http://localhost:5000/api/xml => xml
            [HttpGet("[controller]/{format=json}")]
            [ApiVersion("1.0")]
            [FormatFilter]
            public Model Get()
            {
                return new Model();
            }

            //http://localhost:5000/api?api-version=2.0
            [ApiVersion("2.0")]
            [ApiVersion("3.0")]
            [MapToApiVersion("2.0")]
            [MapToApiVersion("3.0")]
            [HttpGet("[controller]")]
            public Model GetV2()
            {
                return new Model();
            }

            [ProducesResponseType(typeof(Model), StatusCodes.Status201Created)]
            [ProducesResponseType(typeof(Model), StatusCodes.Status202Accepted)]
            [ProducesResponseType(typeof(Model), StatusCodes.Status304NotModified)]
            [ProducesDefaultResponseType]
            [HttpPost("[action]")]
            public IActionResult AddOrUpdate(Model model)
            {
                return CreatedAtAction(nameof(AddOrUpdate), model);
            }

            [HttpGet("[action]")]
            [ProducesResponseType(typeof(Model), StatusCodes.Status200OK)]
            public IActionResult Process(string id, int state)
            {
                return Ok();
            }
        }
    }   
}
