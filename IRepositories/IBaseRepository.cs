using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IList<T> GetAll();
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);

    }
}
