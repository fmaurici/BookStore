using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Client : BaseEntity
    {
        public ApplicationUser User { get; set; }
        public virtual IList<BookClient> BookClients { get; set; }
    }
}
