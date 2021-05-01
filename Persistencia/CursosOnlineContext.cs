

using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    public  class BooksOnlineContext:IdentityDbContext<Usuario>
    {
        public BooksOnlineContext(DbContextOptions options ):base(options)
        {
       }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<AuthorBook>().HasKey(
                        ci => new { 
                                ci.BookID, ci.AuthorID
                        }
                );
        
        }
        
        public DbSet<Curso> Curso { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Activity> Activity { get; set; }



    }
}
