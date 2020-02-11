using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        // GET: api/Author
        [HttpGet]
        public async Task<IEnumerable<Author>> Get()
        {
            return await _authorRepository.GetAll();
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var author = await _authorRepository.GetById(id);

            return author == null ? NotFound() : (IActionResult)Ok(author);
        }

        // POST: api/Author
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Author author)
        {
            if (ModelState.IsValid)
            {
                await _authorRepository.Insert(author);
                return new CreatedAtRouteResult("CreatedBook", new { id = author.Id });
            }

            return BadRequest(ModelState);

        }

        // PUT: api/Author/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Author author)
        {
            if (ModelState.IsValid)
            {
                await _authorRepository.Update(author, id);
                return Ok(new { id = author.Id });
            }
            return BadRequest(ModelState);

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _authorRepository.Delete(id);
        }
    }
}
