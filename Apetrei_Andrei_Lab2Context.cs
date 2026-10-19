using Apetrei_Andrei_Lab2.Models;
using Microsoft.EntityFrameworkCore;

public class Apetrei_Andrei_Lab2Context(DbContextOptions<Apetrei_Andrei_Lab2Context> options) : DbContext(options)
{
    public DbSet<Apetrei_Andrei_Lab2.Models.Book> Book { get; set; } = default!;
    public DbSet<Publisher> Publisher { get; set; } = default!;
    public DbSet<Author> Author { get; set; } = default!;
    public DbSet<Category> Category { get; set; } = default!;
    public DbSet<BookCategory> BookCategory { get; set; } = default!;
}
