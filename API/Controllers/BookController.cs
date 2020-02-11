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
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        //Constructor
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        // GET: api/Book
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            return _bookRepository.GetAll().Result;
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var book = await _bookRepository.GetById(id);

            return book == null ? NotFound() : (IActionResult)Ok(book); //Si no existe retorno NotFound, si existe retorno Ok de book
        }

        // POST: api/Book
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookRepository.Insert(book);
                return new CreatedAtRouteResult("CreatedBook", new { id = book.Id });
            }

            return BadRequest(ModelState);
        }
       
        // PUT: api/Book/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookRepository.Update(book, id);
                return Ok(new { id = book.Id });
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _bookRepository.Delete(id);
        }

        [HttpPut("Rent/{id}")]
        public async Task<IActionResult> Rent(Guid id)
        {
            try
            {
                var newStock = await _bookRepository.Rent(id);
                return Ok(new { id = id, stock = newStock });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
