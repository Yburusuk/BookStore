using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    public DbSet<Book> Books { get; set; }
}