using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUD3.Models;
using Entities;
using IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUD3.Views.Home
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IActionResult Book()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View("Book", new BookViewModel());
        }

        public IActionResult Edit(Guid id)
        {
            var book = _bookRepository.GetById(id);
            var viewModel = FillBookViewModel(book);

            return View("Book", viewModel);
        }

        public IActionResult Delete(Guid id)
        {
            _bookRepository.Delete(id);

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public IActionResult CreateOrEdit(BookViewModel model)
        {
            var book = FillBook(model);

            if (model.Id == new Guid()) 
                CreateBook(book); 
            EditBook(book);

            return RedirectToAction("Index", "Home");
        }

        public void CreateBook(Book book)
        {
            _bookRepository.Create(book);
        }

        public void EditBook(Book book)
        {
            _bookRepository.Edit(book);
        }

        //TODO: pasar a un helper o servicio
        public Book FillBook(BookViewModel bookViewModel)
        {
            var book = new Book { Id = bookViewModel.Id, Name = bookViewModel.Name, Price = bookViewModel.Price, Stock = bookViewModel.Stock };

            return book;
        }

        public BookViewModel FillBookViewModel(Book book)
        {
            var clientList = new List<SelectListItem>();

            if (book.BookClients.Any())
            {
                foreach (Client client in book.BookClients.Select(c => c.Client))
                {
                    clientList.Add(new SelectListItem { Value = client.Id.ToString(), Text = client.Name });
                }
            }
            var bookViewModel = new BookViewModel { Id = book.Id, Name = book.Name, Price = book.Price, Stock = book.Stock, Clients = clientList};

            return bookViewModel;
        }
    }
}