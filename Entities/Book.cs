using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Author Author { get; set; }
        public virtual IList<BookClient> BookClients { get; set; }
    }
}
