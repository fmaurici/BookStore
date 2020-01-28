using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using CRUD3.Models;
using Entities;
using IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repositories;
using static Business.Enums;

namespace CRUD3.Views.Home
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        //Constructor
        public BookController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        //---------- Llamadas para mostrar Views ------------
        public IActionResult BookList()
        {
            //Traigo todos mis books y luego lleno los viewModels con el Select y AutoMapper
            var bookList = _bookRepository.GetAll();

            var viewModel = bookList.Select(book => _mapper.Map<BookViewModel>(book));

            return View(viewModel);
        }

        public IActionResult Add()
        {
            return View("AddBook", new BookViewModel() { Action = Actions.Add.ToString() });
        }

        public IActionResult Edit(Guid id)
        {
            var book = _bookRepository.GetById(id);

            //Lleno mi ViewModel automaticamente con AutoMapper
            var viewModel = _mapper.Map<BookViewModel>(book);
            viewModel.Action = Actions.Edit.ToString();
            return View("AddBook", viewModel);
        }

        //---------- Acciones dentro de las Views ---------------
        [HttpPost]
        public IActionResult CreateOrEdit(BookViewModel model)
        {
            // Convierto mi ViewModel en Entity con Automapper
            var book = _mapper.Map<Book>(model); ;

            //Si mi entidad no existe (osea, su Guid es igual a un New Guid), entonces voy a CreateBook. Si mi entidad existe, entonces voy a EditBook
            if (model.Id == new Guid())
                CreateBook(book);
            EditBook(book);

            return RedirectToAction("BookList", "Book");
        }

        public void CreateBook(Book book)
        {
            _bookRepository.Insert(book);
        }

        public void EditBook(Book book)
        {
            _bookRepository.Update(book);
        }

        public IActionResult Alquilar(Guid id)
        {
            _bookRepository.Alquilar(id);

            return RedirectToAction("BookList", "Book");
        }
        public IActionResult Devolver(Guid id)
        {
            _bookRepository.Devolver(id);

            return RedirectToAction("BookList", "Book");
        }

        public IActionResult Delete(Guid id)
        {
            _bookRepository.Delete(id);

            return RedirectToAction("BookList", "Book");
        }


    }
}