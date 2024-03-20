using FinderBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Concurrent;

namespace FinderBot.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Page> Pages => Set<Page>();
        public DbSet<URL> VisitedUrls => Set<URL>();
        public DbSet<URL> UrlsToCrawl => Set<URL>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
