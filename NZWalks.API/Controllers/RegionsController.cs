using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using NZWalks.API.Walks;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDBContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDBContext dbContext,IRegionRepository regionRepository,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Gets Data from database (domain models)
            var regionsDomain = await regionRepository.GetAllAsync();

            // Maps the Domain to DTO and returns
            return Ok(mapper.Map<List<Region>>(regionsDomain));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Find() only takes the primary key so you can use linq to be more flexible
            var regionsDomain = await regionRepository.GetRegionByIdAsync(id);
            //var region = dbContext.Regions.Find(id);
            if(regionsDomain == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<RegionDTO>(regionsDomain));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            if(ModelState.IsValid)
            {
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);

                await regionRepository.CreateAsync(regionDomainModel);
                var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
            
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            if(ModelState.IsValid)
            {
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDTO);
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<RegionDTO>(regionDomainModel));
            }
            else
            {
                return BadRequest(ModelState);
            }
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
            return Ok(mapper.Map<RegionDTO>(regionDomainModel));
        }
    }
}
