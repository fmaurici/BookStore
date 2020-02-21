using System;
using System.Collections.Generic;
using System.Text;
using static Entities.Enums;

namespace Entities
{
    public class BookOperation : BaseEntity
    {
        public BookOperation(){}

        public BookOperation(ApplicationUser user, Book book, BookOperations operationType)
        {
            User = user;
            Book = book;
            Type = operationType;
            Date = DateTime.Now;
        }

        public ApplicationUser User { get; set; }
        public Book Book { get; set; }
        public BookOperations Type { get; set; }
        public DateTime Date { get; set; }
    }
}
