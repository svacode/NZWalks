using Microsoft.EntityFrameworkCore;
using UdemyProject.Data;
using UdemyProject.Models.Domain;

namespace UdemyProject.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }
        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }
        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingModel == null)
            {
                return null;
            }
            existingModel.Name = region.Name;
            existingModel.Code = region.Code;
            existingModel.RegionImageUrl = region.RegionImageUrl;
            await dbContext.SaveChangesAsync();
            return existingModel;
        }
        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(existingModel == null)
            {
                return null;
            }
            dbContext.Regions.Remove(existingModel);
            await dbContext.SaveChangesAsync();
            return existingModel;
        }
    }
}
