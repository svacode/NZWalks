using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAll()
        {
            var regionsDomain = dbContext.Regions.ToList();
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
        public IActionResult GetById([FromRoute] Guid id)
        {
            var regionDomain = dbContext.Regions.Find(id);
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
        public IActionResult CreateRegion([FromRoute] AddRegionRequestDto addRegionRequestDto)
        {
            //map dto to domain
            var newRegionDomain = new Region()
            {
                Name = addRegionRequestDto.Name,
                Code = addRegionRequestDto.Code,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            };
            //add region to db and save
            dbContext.Add(newRegionDomain);
            dbContext.SaveChanges();
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
        public IActionResult UpdateRegions([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            //var updatedRegionDomain = dbContext.Regions.Find(id);
            var updatedRegionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (updatedRegionDomain == null)
            {
                return NotFound();
            }
            updatedRegionDomain.Name = updateRegionRequestDto.Name;
            updatedRegionDomain.Code = updateRegionRequestDto.Code;
            updatedRegionDomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            dbContext.SaveChanges();

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
        public IActionResult DeleteRegions([FromRoute] Guid id)
        {
            
            var deletedRegion = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(deletedRegion == null)
            {
                return NotFound();
            }
            dbContext.Regions.Remove(deletedRegion);
            dbContext.SaveChanges();
            return Ok(GetAll());
        }
























    }
}