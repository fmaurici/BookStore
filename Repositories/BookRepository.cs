using Database;
using Entities;
using IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly BookContext _context;

        public BookRepository(BookContext context) : base(context)
        {
            _context = context;
        }

        public override Book GetById(object id)
        {
            return _context.Books.Where(b => b.Id == (Guid)id)
                .Include(b => b.BookClients)
                .ThenInclude(b => b.Client)
                .FirstOrDefault();
               
        }
    }
}
