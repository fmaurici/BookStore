using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IRepositories
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        //Como hereda de IBaseRepository<Book>, quiere decir que voy a poder hacer todas las operaciones que hay en IBaseRepository donde T sea un Book
        //Como heredo todos esos metodos, no necesito escribirlos acá, ya que estan en el IBaseRepository<Book> (como lo de baseEntity)

        public Task<int> Rent(Guid id);
        public Task<int> Return(Guid id);
    }


}
