using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CRUD3.Models;
using IRepositories;
using Entities;

namespace CRUD3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookRepository _bookRepository;

        public HomeController(ILogger<HomeController> logger, IBookRepository bookRepository)
        {
            _logger = logger;
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            var books = _bookRepository.GetAll();
            var viewModel = FillBookViewModel(books);

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //TODO: pasar a un helper o servicio
        public IList<BookViewModel> FillBookViewModel(IList<Book> books)
        {
            var booksViewModel = new List<BookViewModel>();

            foreach (Book book in books)
            {
                var bookViewModel = new BookViewModel { Id = book.Id, Name = book.Name, Price = book.Price, Stock = book.Stock };
                booksViewModel.Add(bookViewModel);
            }

            return booksViewModel;
        }

    }
}
