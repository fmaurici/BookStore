using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using IBusiness;
using IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookManager _bookManager;

        //Constructor
        public BookController(IBookRepository bookRepository, IBookManager bookManager)
        {
            _bookRepository = bookRepository;
            _bookManager = bookManager;
        }

        // GET: api/Book
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _bookRepository.GetAll());
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
                var newStock = await _bookManager.Rent(id);
                return Ok(new { id = id, stock = newStock });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Return/{id}")]
        public async Task<IActionResult> Return(Guid id)
        {
            try
            {
                var newStock = await _bookManager.Return(id);
                return Ok(new { id = id, stock = newStock });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
