using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdemyProject.Data;
using UdemyProject.Models.Domain;
using UdemyProject.Models.DTOs;
using UdemyProject.Repositories;

//namespace UdemyProject.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RegionsController : ControllerBase
//    {
//        private readonly NZWalksDbContext dbContext;

//        public RegionsController(NZWalksDbContext dbContext)
//        {
//            this.dbContext = dbContext;
//        }

//        [HttpGet]
//        public IActionResult GetAll()
//        {
//            var regionsDomain = dbContext.Regions.ToList();
//            var regionsDto = new List<RegionDto>();
//            foreach (var region in regionsDomain)
//            {
//                regionsDto.Add(new RegionDto()
//                {
//                    Id = region.Id,
//                    Code = region.Code,
//                    Name = region.Name,
//                    RegionImageUrl = region.RegionImageUrl,
//                });
//            }
//            return Ok(regionsDto);
//        }

//        [HttpGet]
//        [Route("{id:Guid}")]
//        public IActionResult GetById([FromRoute] Guid id)
//        {
//            var regionDomain = dbContext.Regions.Find(id);
//            if (regionDomain == null)
//            {
//                return BadRequest();
//            }
//            var regionDto = new RegionDto()
//            {
//                Id = regionDomain.Id,
//                Code = regionDomain.Code,
//                Name = regionDomain.Name,
//                RegionImageUrl = regionDomain.RegionImageUrl,
//            };

//            return Ok(regionDto);
//        }

//        [HttpPost]
//        public IActionResult AddRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
//        {
//            if (addRegionRequestDto == null)
//            {
//                return BadRequest();
//            }
//            //map dto to domain
//            var RegionDomainModel = new Region()
//            {
//                Code = addRegionRequestDto.Code,
//                Name = addRegionRequestDto.Name,
//                RegionImageUrl = addRegionRequestDto.RegionImageUrl,
//            };

//            //use domain model to create region
//            dbContext.Regions.Add(RegionDomainModel);
//            dbContext.SaveChanges();
//            //map domain model back to dto
//            var regionDto = new RegionDto()
//            {
//                Id = RegionDomainModel.Id,
//                Code = RegionDomainModel.Code,
//                Name = RegionDomainModel.Name,
//                RegionImageUrl = RegionDomainModel.RegionImageUrl,
//            };
//            return CreatedAtAction(nameof(GetById),new { id = RegionDomainModel.Id }, regionDto);

//        }
//    }
//}

// Rewriting

namespace UdemyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //you continue working while I wait for my query results
            //using interface to bring out data
            var regionsDomain = await regionRepository.GetAllAsync();
            //mapping domain to dto
            var regionsDto = new List<RegionDto>();
            foreach (var region in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }

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
            //the mapping
            var regionDto = new Region()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };
            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromRoute] AddRegionRequestDto addRegionRequestDto)
        {
            //map dto to domain
            var newRegionDomain = new Region()
            {
                Name = addRegionRequestDto.Name,
                Code = addRegionRequestDto.Code,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            };
            //add region to db and save
            await regionRepository.CreateAsync(newRegionDomain);
            //map domain back to dto
            var regionDto = new RegionDto()
            {
                Id = newRegionDomain.Id,
                Name = newRegionDomain.Name,
                Code = newRegionDomain.Code,
                RegionImageUrl = newRegionDomain.RegionImageUrl,

            };
            return CreatedAtAction(nameof(GetById), new { id = newRegionDomain.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegions([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = new Region()
            {
                Name = updateRegionRequestDto.Name,
                Code = updateRegionRequestDto.Code,
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl,
            };
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            var updatedRegionDto = new RegionDto()
            {
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
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
            var deletedModelDto = new RegionDto()
            {
                Name = deletedRegion.Name,
                Code = deletedRegion.Code,
                RegionImageUrl = deletedRegion.RegionImageUrl,
            };
            return Ok(deletedModelDto);
        }
























    }
}