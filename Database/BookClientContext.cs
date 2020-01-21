using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class BookClientContext : DbContext
    {
        public BookClientContext(DbContextOptions<BookClientContext> options) : base(options)
        {
        }

        public DbSet<BookClient> BookClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Para configurar la relación many to many con los nombres con convensión + los DBSet que hay arriba
            modelBuilder.Entity<BookClient>().HasKey(bc => new { bc.BookId, bc.ClientId });
        }
    }
}
