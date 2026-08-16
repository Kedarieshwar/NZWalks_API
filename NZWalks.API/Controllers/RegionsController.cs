using NZWalks.API.Models.DTO;
using NZWalks.API.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Walks;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDBContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDBContext dbContext,IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllAsync();
            var regionDTO = new List<RegionDTO>();
            if (regions == null)
            {
                return NotFound();
            }
            foreach (var region in regions)
            {
                regionDTO.Add(
                    new RegionDTO()
                    {
                        Id = region.Id,
                        Code = region.Code,
                        Name = region.Name,
                        RegionImageUrl = region.RegionImageUrl,
                    }
                );
            }

            return Ok(regionDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Find() only takes the primary key so you can use linq to be more flexible
            var regions = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            //var region = dbContext.Regions.Find(id);
            var regionDTO = new RegionDTO()
            {
                Id = regions.Id,
                Code = regions.Code,
                Name = regions.Name,
                RegionImageUrl = regions.RegionImageUrl,
            };
                
            
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
            };

            await regionRepository.CreateAsync(regionDomainModel);
            var regionDTO = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            var regionDomainModel = new Region
            {
                Name = updateRegionRequestDTO.Name,
                Code = updateRegionRequestDTO.Code,
                RegionImageUrl = updateRegionRequestDTO.RegionImageUrl,

            };
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            var regionDTO = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return Ok(regionDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            return Ok(regionDto);
        }
    }
}
