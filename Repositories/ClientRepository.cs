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
        private readonly BookContext _context;

        public ClientRepository(BookContext context) : base(context)
        {
            _context = context;
        }
    }
}
