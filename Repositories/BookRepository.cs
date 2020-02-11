using Database;
using Entities;
using IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly BookStoreContext _context;
        private readonly IAuthorRepository _authorRepository;

        //Inyectamos el contexto (es decir, la base de datos para que entity Framework la pueda usar acá)
        public BookRepository(BookStoreContext context, IAuthorRepository authorRepository) : base(context)
        {
            _context = context;
            _authorRepository = authorRepository;
        }

        public async override Task<Book> GetById(object id)
        {
            return await _context.Books.Where(b => b.Id == (Guid)id)
                .Include(b => b.Author)
                .Include(b => b.BookClients)
                .ThenInclude(bc => bc.Client)
                .FirstOrDefaultAsync();
               
        }

        public async override Task<IList<Book>> GetAll()
        {
            //Los includes lo que hacen es traer datos anidados que no estén directo en la tabla Books (como hacemos con los inner join en SQL)
            //Entonces acá decimos, Traeme los libros que esten en nuestro contexto (base de datos) y también incluime los BookClients hijos y los Clients de esos BookClients
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookClients)
                .ThenInclude(bc => bc.Client)
                .ToListAsync();
        }

        public async override Task Insert(Book obj)
        {
            var book = obj;
            if (book.Author != null)
            {
                var author = await _authorRepository.GetById(book.Author.Id);
                book.Author = author;
            }
            
            _context.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task<int> Rent(Guid id)
        {
            var book = await GetById(id);

            if(book == null) { throw new Exception("Book Id " + id + " not found"); }

            book.Rent();
            await Update(book, id);
            return book.Stock;
        }
        public async Task<int> Return(Guid id)
        {
            var book = await GetById(id);
            book.Return();
            await Update(book);
            return book.Stock;
        }
    }
}
