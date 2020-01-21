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
    public class BookRepository : IBookRepository
    {
        private readonly BookContext _context;

        public BookRepository(BookContext context)
        {
            _context = context;
        }

        public void Create(Book book)
        {
            _context.Add(book);
            _context.SaveChanges();
        }

        public void Edit(Book book)
        {
            _context.Update(book);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var book = GetById(id);
            _context.Remove(book);
            _context.SaveChanges();
        }

        public IList<Book> GetAll()
        {
            return _context.Books.ToList();
        }

        public Book GetById(Guid id)
        {
            return _context.Books.Where(b => b.Id == id)
                .Include(b => b.BookClients)
                .ThenInclude(b => b.Client)
                .FirstOrDefault();
               
        }
    }
}
