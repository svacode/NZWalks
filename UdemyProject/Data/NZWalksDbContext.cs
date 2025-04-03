using Microsoft.EntityFrameworkCore;
using UdemyProject.Models.Domain;

namespace UdemyProject.Data
{
    public class NZWalksDbContext : DbContext
    {
        //private string readonly _dbContext;
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> DbContextOptions) : base(DbContextOptions)
        {
            
        }
        public DbSet <Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}
