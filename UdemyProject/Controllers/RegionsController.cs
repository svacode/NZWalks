using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdemyProject.Data;
using UdemyProject.Models.Domain;
using UdemyProject.Models.DTOs;

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
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await dbContext.Regions.ToListAsync(); //you continue working while I wait for my query results
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
            var regionDomain = await dbContext.Regions.FindAsync(id);
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
            await dbContext.AddAsync(newRegionDomain);
            await dbContext.SaveChangesAsync();
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

            //var updatedRegionDomain = dbContext.Regions.Find(id);
            var updatedRegionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (updatedRegionDomain == null)
            {
                return NotFound();
            }
            updatedRegionDomain.Name = updateRegionRequestDto.Name;
            updatedRegionDomain.Code = updateRegionRequestDto.Code;
            updatedRegionDomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            await dbContext.SaveChangesAsync();

            var updatedRegionDto = new RegionDto()
            {
                Name = updatedRegionDomain.Name,
                Code = updatedRegionDomain.Code,
                RegionImageUrl = updatedRegionDomain.RegionImageUrl,
            };
            return Ok(updatedRegionDto);

        }

        [HttpDelete]
        [Route("{id:Guid})")]
        public async Task<IActionResult> DeleteRegions([FromRoute] Guid id)
        {
            
            var deletedRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(deletedRegion == null)
            {
                return NotFound();
            }
            dbContext.Regions.Remove(deletedRegion);
            await dbContext.SaveChangesAsync();
            return Ok(GetAll());
        }
























    }
}