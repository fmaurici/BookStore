using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRUD3.Models;
using Entities;
using IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace CRUD3.Controllers
{
    public class BookOperationController : Controller
    {
        private readonly IBookOperationRepository _bookOperationRepository;
        private readonly IMapper _mapper;

        public BookOperationController(IBookOperationRepository bookOperationRepository, IMapper mapper)
        {
            _bookOperationRepository = bookOperationRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            var bookOperations = await _bookOperationRepository.FindBy(x => x.Book.Id == id);

            var viewModel = bookOperations.Select(bookOperation => _mapper.Map<BookOperationViewModel>(bookOperation));

            return View("BookOperationList", viewModel);
        }
    }
}