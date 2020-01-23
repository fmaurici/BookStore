using Database;
using Entities;
using IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        private readonly BookStoreContext _context;

        public AuthorRepository(BookStoreContext context) : base(context)
        {
            _context = context;
        }

    }
}
