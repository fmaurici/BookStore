using Database;
using Entities;
using IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity //BaseRepository hereda de IBaseRepository de T, donde T es una BaseEntity (es decir cualquier clase que herede de BaseEntity)
    {
        private readonly BookStoreContext _context;

        public BaseRepository(BookStoreContext context)
        // el context este se encuentra en el startup para conectar con entity framework core 
        {
            _context = context;
        }

        //En este base repo van a ir todos los metodos que se utilicen en TODOS los repositorios de la misma forma. 
        //Si queremos usar alguno de forma especial, solo lo overraideamos en el repo hijo (Como el GetAll() en BookRepository)
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
