using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IRepositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IList<T>> GetAll();
        Task<T> GetById(object id);
        Task Insert(T obj);
        Task Update(T obj, Guid id = new Guid());
        Task Delete(object id);

    }
}
