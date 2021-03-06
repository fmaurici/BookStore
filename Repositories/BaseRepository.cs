﻿using Database;
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
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity //BaseRepository hereda de IBaseRepository de T, donde T es una BaseEntity (es decir cualquier clase que herede de BaseEntity)
    {
        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //En este base repo van a ir todos los metodos que se utilicen en TODOS los repositorios de la misma forma. 
        //Si queremos usar alguno de forma especial, solo lo overraideamos en el repo hijo (Como el GetAll() en BookRepository)
        public virtual async Task<IList<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async virtual Task<T> GetById(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async virtual Task Insert(T obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async virtual Task Update(T obj, Guid id = new Guid())
        {
            if(obj.Id == new Guid())
            {
                obj.Id = id;
            }

            _context.Update(obj);
            await _context.SaveChangesAsync();
        }

        public async virtual Task Delete(object id)
        {
            var obj = await GetById(id);
            _context.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async virtual Task<IList<T>> FindBy(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

    }
}
