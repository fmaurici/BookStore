using Business;
using Entities;
using IBusiness;
using IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class BookTest
    {
        [Fact]
        public void BookManager_Rent_ReduceStock()
        {
            //Mockeo las dependencias que no quiero testear
            var bookRepository = new Mock<IBookRepository>();
            var bookOperationRepository = new Mock<IBookOperationRepository>();
            var httpContextAccesor = new Mock<IHttpContextAccessor>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            //Creo los objetos dummy a utilizar en los tests
            var book = new Book() { Id = new Guid(), Name = "TestBook", Stock = 10, BookClients = new List<BookClient>() };
            var user = new ApplicationUser() { Id = new Guid(), Email = "test@test.com" };
            var bookOperation = new BookOperation() { Book = book, Date = DateTime.Now, Type = Enums.BookOperations.Rent, User = user };
            var claim = new ClaimsPrincipal();

            //Hago un setup para indicarle a los metodos de las clases mockeadas que no quiero testear, lo que deben devolver
            bookRepository.Setup(x => x.GetById(new Guid())).ReturnsAsync(book);
            bookRepository.Setup(x => x.Update(book, new Guid())).Returns(Task.FromResult(book));
            httpContextAccesor.Setup(x => x.HttpContext.User).Returns(claim);
            userManager.Setup(x => x.GetUserAsync(claim)).ReturnsAsync(user);
            bookOperationRepository.Setup(x => x.Insert(bookOperation)).Returns(Task.FromResult(bookOperation));

            //Instancio la clase a testear
            var bookManager = new BookManager(bookRepository.Object, bookOperationRepository.Object, userManager.Object, httpContextAccesor.Object);

            //Ejecuto el metodo a testear
            var result = bookManager.Rent(book.Id).Result;

            //Hago el Assert para chequear que haya llegado bien
            Assert.Equal(9, result);

            //TODO: ver si puedo hacer inyeccion de dependencias en xUnit. Mover la creacion de los objetos dummy a un fixture 
        }
    }
}
