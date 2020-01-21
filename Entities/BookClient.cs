using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class BookClient
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
    }
}
