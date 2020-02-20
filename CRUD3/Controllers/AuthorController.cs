using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRUD3.Models;
using Entities;
using IRepositories;
using Microsoft.AspNetCore.Mvc;
using static Entities.Enums;

namespace CRUD3.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        //Constructor
        public AuthorController(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> AuthorList()
        {
            var authorList = await _authorRepository.GetAll();
            var viewModel = authorList.Select(author => _mapper.Map<AuthorViewModel>(author));

            return View(viewModel);
        }

        public IActionResult Add()
        {
            return View("AddAuthor", new AuthorViewModel() { Action = Actions.Add.ToString() });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var author = await _authorRepository.GetById(id);
            
            var viewModel = _mapper.Map<AuthorViewModel>(author);
            viewModel.Action = Actions.Edit.ToString();

            return View("AddAuthor", viewModel);
        }

        [HttpPost]
        public IActionResult CreateOrEdit(AuthorViewModel model)
        {
            var author = _mapper.Map<Author>(model); ;

            if (model.Id == new Guid())
                CreateAuthor(author);
            EditAuthor(author);

            return RedirectToAction("AuthorList", "Author");
        }

        public void CreateAuthor(Author author)
        {
            _authorRepository.Insert(author);
        }

        public void EditAuthor(Author author)
        {
            _authorRepository.Update(author);
        }

        public IActionResult Delete(Guid id)
        {
            _authorRepository.Delete(id);

            return RedirectToAction("AuthorList", "Author");
        }
    }
}