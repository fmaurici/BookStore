using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {
        }

        //Le decimos a Entity Framework que para nuestra relacion muchos a muchos vamos a usar estas 3 tablas
        public DbSet<Book> Books { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookClient> BookClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Esto es para poder configurar la relacion muchos a muchos entre Books y Clientes en Entity Framework
            //(Cada book puede tener muchos clients, y cada client puede tener muchos books)
            modelBuilder.Entity<BookClient>().HasKey(bc => new { bc.BookId, bc.ClientId });

            modelBuilder.Entity<BookClient>()
                .HasOne<Book>(b => b.Book)
                .WithMany(c => c.BookClients)
                .HasForeignKey(b => b.BookId);

            modelBuilder.Entity<BookClient>()
                .HasOne<Client>(c => c.Client)
                .WithMany(c => c.BookClients)
                .HasForeignKey(c => c.ClientId);
        }
    }
}
