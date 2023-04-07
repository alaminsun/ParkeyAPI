using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkeyAPI.Models;
using ParkeyAPI.Models.Dtos;
using ParkeyAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParkeyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailsController : Controller
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
        /// <param name="TrailId">The Id of the trail</param>
        /// <returns></returns>
        [HttpGet("{TrailId:int}", Name = "GetTrail")]
        public IActionResult GetTrail(int TrailId)
        {
            var obj = _trailRepo.GetTrail(TrailId);
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

        [HttpPost]
        public IActionResult CreateTrail([FromBody] TrailDto TrailDto)
        {
            if (TrailDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepo.TrailExists(TrailDto.Name))
            {
                ModelState.AddModelError("", "Trail Park Exists!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var TrailObj = _mapper.Map<Trail>(TrailDto);

            if (!_trailRepo.CreateTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when save the record {TrailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { TrailId = TrailObj.Id }, TrailObj);
        }

        [HttpPatch("{TrailId:int}", Name = "UpdateTrail")]
        public IActionResult UpdateTrail(int TrailId, [FromBody] TrailDto TrailDto)
        {
            if (TrailDto == null || TrailId != TrailDto.Id)
            {
                return BadRequest(ModelState);
            }
            //if (_trailRepo.TrailExists(TrailDto.Name))
            //{
            //    ModelState.AddModelError("", "Trail Exists!");
            //    return StatusCode(404, ModelState);
            //}
            var TrailObj = _mapper.Map<Trail>(TrailDto);

            if (!_trailRepo.UpadteTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {TrailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{TrailId:int}", Name = "DeleteTrail")]
        public IActionResult DeleteTrail(int TrailId)
        {
            if (!_trailRepo.TrailExists(TrailId))
            {
                return NotFound();
            }
            var TrailObj = _trailRepo.GetTrail(TrailId);

            if (!_trailRepo.DeleteTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {TrailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
