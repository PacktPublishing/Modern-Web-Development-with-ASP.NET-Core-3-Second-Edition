using chapter08.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace chapter08.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class PetController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] Pet pet)
        {
            return new JsonResult(new { Ok = true });
        }

        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        [HttpPut("{id}")]
        public async Task<ActionResult<Pet>> PutPet(int id, Pet pet)
        {
            return Ok();
        }
    }
}
