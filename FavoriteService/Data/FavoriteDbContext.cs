using Microsoft.EntityFrameworkCore;
using FavoriteService.Models;

namespace FavoriteService.Data
{
    public class FavoriteDbContext : DbContext
    {
        public FavoriteDbContext(DbContextOptions<FavoriteDbContext> options) : base(options) { }
        public DbSet<FavoriteItem> FavoriteItems { get; set; }
    }
}
