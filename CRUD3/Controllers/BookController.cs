using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using CRUD3.Models;
using Entities;
using IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repositories;
using static Business.Enums;

namespace CRUD3.Views.Home
{
    //Con este action filter restrinjo el acceso a todos los action del controller 
    //(tambien se puede poner solo en actions específicos, arriba de cada uno de ellos)
    [Authorize]
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
        public async Task<IActionResult> BookList()
        {
            //Traigo todos mis books y luego lleno los viewModels con el Select y AutoMapper
            var bookList = await _bookRepository.GetAll();

            var viewModel = bookList.Select(book => _mapper.Map<BookViewModel>(book));

            return View(viewModel);
        }

        public IActionResult Add()
        {
            return View("AddBook", new BookViewModel() { Action = Actions.Add.ToString() });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var book = await _bookRepository.GetById(id);

            //Lleno mi ViewModel automaticamente con AutoMapper
            var viewModel = _mapper.Map<BookViewModel>(book);
            viewModel.Action = Actions.Edit.ToString();
            return View("AddBook", viewModel);
        }

        //---------- Acciones dentro de las Views ---------------
        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(BookViewModel model)
        {
            // Convierto mi ViewModel en Entity con Automapper
            var book = _mapper.Map<Book>(model);

            //Si mi entidad no existe (osea, su Guid es igual a un New Guid), entonces voy a CreateBook. Si mi entidad existe, entonces voy a EditBook
            if (model.Id == new Guid()) //TODO: cambiar por GUID.Empty
            {
                await CreateBook(book);
            }
            else
            {
                await EditBook(book);
            }

            return RedirectToAction("BookList", "Book");
        }

        public async Task CreateBook(Book book)
        {
           await _bookRepository.Insert(book);
        }

        public async Task EditBook(Book book)
        {
            await _bookRepository.Update(book);
        }

        public async Task<IActionResult> Rent(Guid id)
        {
            try
            {
                var result = await _bookRepository.Rent(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Return(Guid id)
        {
            try
            {
                var result = await _bookRepository.Return(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
                       
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookRepository.Delete(id);
            return RedirectToAction("BookList", "Book");
        }
    }
}