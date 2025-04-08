using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdemyProject.CustomActionFilters;
using UdemyProject.Data;
using UdemyProject.Models.Domain;
using UdemyProject.Models.DTOs;
using UdemyProject.Repositories;

namespace UdemyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegionsController : ControllerBase
    {
        //private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(/*NZWalksDbContext dbContext,*/ IRegionRepository regionRepository, IMapper mapper)
        {
            //this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //you continue working while I wait for my query results
            //using interface to bring out data
            var regionsDomain = await regionRepository.GetAllAsync();
            //mapping domain to dto
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return BadRequest();
            }
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateRegion([FromRoute] AddRegionRequestDto addRegionRequestDto)
        {        
                var newRegionDomain = mapper.Map<Region>(addRegionRequestDto);
                //add region to db and save
                newRegionDomain = await regionRepository.CreateAsync(newRegionDomain);
                var regionDto = mapper.Map<RegionDto>(newRegionDomain);
                return CreatedAtAction(nameof(GetById), new { id = newRegionDomain.Id }, regionDto);
            
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateRegions([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
           
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }
                var updatedRegionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(updatedRegionDto);  
        }

        [HttpDelete]
        [Route("{id:Guid})")]
        public async Task<IActionResult> DeleteRegions([FromRoute] Guid id)
        {
            
            var deletedRegion = await regionRepository.DeleteAsync(id);
            if(deletedRegion == null)
            {
                return NotFound();
            }
            var deletedModelDto = mapper.Map<RegionDto>(deletedRegion);
            return Ok(deletedModelDto);
        }


    }
}