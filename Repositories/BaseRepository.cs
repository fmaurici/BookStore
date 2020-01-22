using Database;
using Entities;
using IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly BookContext _context;

        public BaseRepository(BookContext context)
        // el context este se encuentra en el startup para conectar con entity framework core 
        {
            _context = context;
        }

        public virtual IList<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public virtual T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual void Insert(T obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public virtual void Update(T obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            var obj = GetById(id);
            _context.Remove(obj);
            _context.SaveChanges();
        }

    }
}
