using chapter08.Controllers.Controllers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chapter08.Controllers
{
    namespace Controllers.V1
    {
        public class ApiController : ControllerBase
        {
            //http://localhost:5000/api/Get/1.0/json => default json
            //http://localhost:5000/api/Get/1.0/xml => xml
            [HttpGet("[controller]/[action]/{version:apiversion}/{format=json}")]
            [FormatFilter]
            public Model Get()
            {
                return new Model();
            }

            [ApiVersion("2.0")]
            [ApiVersion("3.0")]
            [HttpGet("[controller]/[action]/{version:apiversion}")]
            public Model GetV2()
            {
                return new Model();
            }



            [ProducesResponseType(typeof(Model), StatusCodes.Status201Created)]
            [ProducesResponseType(typeof(Model), StatusCodes.Status202Accepted)]
            [ProducesResponseType(typeof(Model), StatusCodes.Status304NotModified)]
            [ProducesDefaultResponseType]
            public IActionResult AddOrUpdate(Model model)
            {
                return CreatedAtAction(nameof(AddOrUpdate), model);
            }

            [HttpGet("Process")]
            [ProducesResponseType(typeof(Model), StatusCodes.Status200OK)]
            public IActionResult Process(string id, int state)
            {
                return Ok();
            }
        }
    }   
}
