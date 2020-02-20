using Entities;
using IBusiness;
using IBusiness.Account;
using IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using static Entities.Enums;
using System.Linq;

namespace Business.Book
{
    public class BookManager : IBookManager
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookOperationRepository _bookOperationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookManager(IBookRepository bookRepository,
            IBookOperationRepository bookOperationRepository,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookRepository = bookRepository;
            _bookOperationRepository = bookOperationRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Rent(Guid id)
        {
            var book = await _bookRepository.GetById(id);

            if (book == null)
            {
                throw new Exception("Book Id " + id + " not found");
            }

            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (book.BookClients.Any(x => x.Client.User.Id == currentUser.Id))
            {
                throw new Exception("Book Id " + id + " is already rented by " + currentUser.UserName + " You must return it before Renting it again");
            }

            //Add Client to Book and ReduceStock
            var bookClient = new BookClient() { Book = book, Client = new Client() { User = currentUser } };
            book.ReduceStockInOne();
            book.BookClients.Add(bookClient);
            await _bookRepository.Update(book, id);

            //Create log of BookOperation
            var bookOperation = new BookOperation(currentUser, BookOperations.Rent);
            await _bookOperationRepository.Insert(bookOperation);
            
            return book.Stock;
        }
    }
}
