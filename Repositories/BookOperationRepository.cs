using Database;
using Entities;
using IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BookOperationRepository : BaseRepository<BookOperation>, IBookOperationRepository
    {
        private readonly ApplicationDbContext _context;

        public BookOperationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IList<BookOperation>> FindBy(Expression<Func<BookOperation, bool>> predicate)
        {
            return await _context.BookOperations.Where(predicate)
                .Include(x => x.Book)
                .Include(x => x.User)
                .ToListAsync();
        }
    }
}
