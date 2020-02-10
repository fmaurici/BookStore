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
        public async Task<Author> Get(int id)
        {
            return await _authorRepository.GetById(id);
        }

        //// POST: api/Author
        //[HttpPost]
        //public void Post([FromBody] Author author)
        //{
        //    _authorRepository.Insert(author);
        //}

        //// PUT: api/Author/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] Author author)
        //{
        //    _authorRepository.Update(author);
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    _authorRepository.Delete(id);
        //}
    }
}
