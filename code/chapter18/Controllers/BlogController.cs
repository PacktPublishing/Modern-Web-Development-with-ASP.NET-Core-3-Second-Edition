using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chapter18.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chapter18.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BlogContext _context;

        public BlogController(BlogContext context)
        {
            this._context = context;
        }

        [HttpGet("{id?}")]
        public async Task<ActionResult<Blog>> Get(int? id = null)
        {
            if (id == null)
            {
                return this.Ok(await this._context.Blogs.AsNoTracking().ToListAsync());
            }
            else
            {
                var blog = await this._context.Blogs.FindAsync(id);

                if (blog == null)
                {
                    return this.NotFound();
                }
                else
                {
                    return this.Ok(blog);
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Blog>> Put(int id, [FromBody] Blog blog)
        {
            if (id != blog.Id)
            {
                return this.BadRequest();
            }

            if (this.ModelState.IsValid)
            {
                this._context.Entry(blog).State = EntityState.Modified;
                try
                {
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return this.Conflict();
                }
                return this.Ok(blog);
            }
            else
            {
                return this.UnprocessableEntity();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await this._context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return this.NotFound();
            }

            this._context.Blogs.Remove(blog);
            await this._context.SaveChangesAsync();
            return this.Accepted();
        }

        [HttpPost]
        public async Task<ActionResult<Blog>> Post([FromBody] Blog blog)
        {
            if (blog.Id != 0)
            {
                return this.BadRequest();
            }

            if (this.ModelState.IsValid)
            {
                this._context.Blogs.Add(blog);
                await this._context.SaveChangesAsync();
                return this.CreatedAtAction(nameof(Post), blog);
            }
            else
            {
                return this.UnprocessableEntity();
            }
        }
    }
}
