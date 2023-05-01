using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkeyAPI.Models;
using ParkeyAPI.Models.Dtos;
using ParkeyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkeyAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkeyOpenAPISpecNP")]
    public class NationalParksV2Controller : ControllerBase
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;
        public NationalParksV2Controller(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }
        /// <summary>
        /// Get list of national parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetNatinalParks()
        {
            var obj = _npRepo.GetNationalParks().FirstOrDefault();
            return Ok(obj);
        }
    }
}
