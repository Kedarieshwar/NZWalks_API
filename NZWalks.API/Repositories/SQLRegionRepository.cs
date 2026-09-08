using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Walks;
namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext dbContext;

        public SQLRegionRepository(NZWalksDBContext dbContext)
        {
            this.dbContext = dbContext; 
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> GetRegionByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion != null)
            {
                 dbContext.Regions.Remove(existingRegion);
                await dbContext.SaveChangesAsync();

                return existingRegion;
            }
            return null;

        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);    
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var targetRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (targetRegion == null)
            {
                return null;
            }
            targetRegion.Code = region.Code;
            targetRegion.Name = region.Name;
            targetRegion.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();
            return targetRegion;
        }
    }
}
