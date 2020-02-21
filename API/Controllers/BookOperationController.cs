using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookOperationController : ControllerBase
    {
        private readonly IBookOperationRepository _bookOperationRepository;
        private readonly IMapper _mapper;

        public BookOperationController(IBookOperationRepository bookOperationRepository, IMapper mapper)
        {
            _bookOperationRepository = bookOperationRepository;
            _mapper = mapper;
        }

        // GET: api/BookOperation
        [HttpGet]
        public async Task<IActionResult> Get(Guid bookId)
        {
            var bookOperations = await _bookOperationRepository.FindBy(x => x.Book.Id == bookId);

            return Ok(bookOperations);

        }
    }
}