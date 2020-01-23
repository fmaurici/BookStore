using Database;
using Entities;
using IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        private readonly BookStoreContext _context;

        public ClientRepository(BookStoreContext context) : base(context)
        {
            _context = context;
        }
    }
}
