using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UdemyProject.Data;
using UdemyProject.Models.DTOs;

namespace UdemyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public WalksController(NZWalksDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
            //map dto to domain model

        }
    }
}
