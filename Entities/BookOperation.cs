using System;
using System.Collections.Generic;
using System.Text;
using static Entities.Enums;

namespace Entities
{
    public class BookOperation : BaseEntity
    {
        public BookOperation()
        {

        }

        public BookOperation(ApplicationUser user, BookOperations operationType)
        {
            User = user;
            Type = operationType;
            Date = DateTime.Now;
        }

        public ApplicationUser User { get; set; }
        public BookOperations Type { get; set; }
        public DateTime Date { get; set; }
    }
}
