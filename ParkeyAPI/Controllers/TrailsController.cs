using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkeyAPI.Models;
using ParkeyAPI.Models.Dtos;
using ParkeyAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParkeyAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkeyOpenAPISpecTrails")]
    public class TrailsController : ControllerBase
    {
        private ITrailRepository _trailRepo;
        private readonly IMapper _mapper;
        public TrailsController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }
        /// <summary>
        /// Get list of trail .
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTrails()
        {
            var objList = _trailRepo.GetTrails();
            var objDto = new List<TrailDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trailId">The Id of the trail</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepo.GetTrail(trailId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDto>(obj);
            //var objDto = new TrailDto()
            //{
            //    Created = obj.Created,
            //    Id = obj.Id,
            //    Name = obj.Name,
            //    State = obj.State
            //};
            return Ok(objDto);
        }

        //[HttpGet("{nationaParkId:int}", Name = "GetTrailInNationalPark")]
        [HttpGet("[action]/{nationaParkId:int}")]
        public IActionResult GetTrailInNationalPark(int nationaParkId)
        {
            var objList = _trailRepo.GetTrailsInNationalPark(nationaParkId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }    
            return Ok(objDto);
        }

        [HttpPost]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when save the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);
        }

        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }
            //if (_trailRepo.TrailExists(TrailDto.Name))
            //{
            //    ModelState.AddModelError("", "Trail Exists!");
            //    return StatusCode(404, ModelState);
            //}
            var trailObj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepo.UpadteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepo.TrailExists(trailId))
            {
                return NotFound();
            }
            var trailObj = _trailRepo.GetTrail(trailId);

            if (!_trailRepo.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
