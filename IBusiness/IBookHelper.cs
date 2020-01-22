using CRUD3.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBusiness
{
    public interface IBookHelper
    {
        public Book FillBook(BookViewModel bookViewModel);

        public BookViewModel FillBookViewModel(Book book);
    }
}
