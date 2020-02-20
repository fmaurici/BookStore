using Database;
using Entities;
using IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class BookOperationRepository : BaseRepository<BookOperation>, IBookOperationRepository
    {
        private readonly ApplicationDbContext _context;

        public BookOperationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
