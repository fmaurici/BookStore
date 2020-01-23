using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public BookController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        //Constructor
        public IActionResult Book()
        {
            return View();
        }

        //---------- Llamadas para mostrar Views ------------
        public IActionResult Add()
        {
            return View("Book", new BookViewModel());
        }

        public IActionResult Edit(Guid id)
        {
            var book = _bookRepository.GetById(id);
            //Lleno mi ViewModel automaticamente con AutoMapper
            var viewModel = _mapper.Map<BookViewModel>(book);

            return View("Book", viewModel);
        }

        //---------- Acciones dentro de las Views ------------
        [HttpPost]
        public IActionResult CreateOrEdit(BookViewModel model)
        {
            // Convierto mi ViewModel en Entity con Automapper
            var book = _mapper.Map<Book>(model); ;

            //Si mi entidad no existe (osea, su Guid es igual a un New Guid), entonces redirijo a CreateBook. Si mi entidad existe, entonces redirijo a EditBook
            if (model.Id == new Guid()) 
                CreateBook(book); 
            EditBook(book);

            return RedirectToAction("Index", "Home");
        }

        public void CreateBook(Book book)
        {
            _bookRepository.Insert(book);
        }

        public void EditBook(Book book)
        {
            _bookRepository.Update(book);
        }

        public IActionResult Delete(Guid id)
        {
            _bookRepository.Delete(id);

            return RedirectToAction("Index", "Home");
        }


    }
}