using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<BookClient> BookClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
