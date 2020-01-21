using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class ClientContext : DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options) : base(options)
        {
        }

        //Esto se crea para poder usar el contexto desde EntityFramework. Hay que crear un Context por cada clase que queramos consultar
        public DbSet<Client> Clients { get; set; }
        public DbSet<BookClient> BookClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookClient>()
                .HasOne<Client>(b => b.Client)
                .WithMany(c => c.BookClients)
                .HasForeignKey(b => b.ClientId);
        }

    }
}
