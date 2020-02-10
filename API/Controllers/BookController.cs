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
        public Book Get(Guid id)
        {
            return _bookRepository.GetById(id).Result;
        }

        // POST: api/Book
        [HttpPost]
        public void Post([FromBody] Book book)
        {
            _bookRepository.Insert(book);
        }


        //TODO: HACER ALQUILAR METHOD

        // PUT: api/Book/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] Book book)
        {
            _bookRepository.Update(book);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _bookRepository.Delete(id);
        }
    }
}
