using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositories
{
    public interface IBookRepository
    {
        IList<Book> GetAll();

        Book GetById(Guid id);

        void Create(Book book);

        void Edit(Book book);

        void Delete(Guid id);
    }
}
