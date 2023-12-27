
using Bookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Data{

    public class ApplicationDbContext : DbContext{

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }

        public DbSet<Category> Categories{get;set;}=null!;
        public DbSet<Book> Books{get;set;}=null!;
        public DbSet<Client> Clients{get;set;}=null!;
        public DbSet<Borrowing> Borrowings{get;set;}=null!;
        public DbSet<Penalty> Penalties{get;set;}=null!;
        public DbSet<User> Users{get;set;}=null!;

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Borrowing>().Property(b => b.Id).ValueGeneratedOnAdd();
        //     // modelBuilder.Entity<Borrowing>().Property(b => b.Id).ValueGeneratedOnAdd();
        // }
    }
}