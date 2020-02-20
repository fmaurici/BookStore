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
        public Editorial Editorial { get; set; }
        public virtual IList<BookClient> BookClients { get; set; }

        public int ReduceStockInOne()
        {
            return this.Stock -= 1;
        }

        public int IncreaseStockInOne()
        {
            return this.Stock += 1;
        }
    }
}
