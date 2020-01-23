using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositories
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        //Como hereda de IBaseRepository<Book>, quiere decir que voy a poder hacer todas las operaciones que hay en IBaseRepository donde T sea un Book
        //Como heredo todos esos metodos, no necesito escribirlos acá, ya que estan en el IBaseRepository<Book> (como lo de baseEntity)

        //Acá debería ir nuestro "Alquilar" sin implementar, que luego va a ser  usado por nuestro BookRepository
    }
}
