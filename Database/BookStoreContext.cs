using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class BookStoreContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {
        }

        //Le decimos a Entity Framework que para nuestra relacion muchos a muchos vamos a usar estas 3 tablas
        public DbSet<Book> Books { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookClient> BookClients { get; set; }
        public DbSet<Editorial> Editorial { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
            });

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
