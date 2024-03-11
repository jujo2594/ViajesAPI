using Asp.Versioning;
using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ViajesAPI.Dto;

namespace ViajesAPI.Controllers;
    [ApiVersion("1")]
    [ApiVersion("1.1")]
    public class JourneyController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public JourneyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<JourneyDto>>> Get()
        {
            var results = await _unitOfWork.Journeys.GetAllAsync();
            return _mapper.Map<List<JourneyDto>>(results);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JourneyDto>> Get(int id)
        {
            var result = await _unitOfWork.Journeys.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return _mapper.Map<JourneyDto>(result);
        }

        [HttpGet("StepsInFlight")]
        [MapToApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Object>>> GetStepsInTrip(string origin, string destination)
        {
            var result = await _unitOfWork.Journeys.GetStepsInTrip(origin, destination);
            if (result.Count() == 0)
            {
                return NotFound("Lo sentimos no tenemos una ruta disponible");
            }
            return _mapper.Map<List<Object>>(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JourneyDto>> PostJourney(JourneyDto journeyDto)
        {
            try
            {
                await _unitOfWork.Journeys.SaveMappInfo();
                return StatusCode(201, "Journeys saved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JourneyDto>> Put(int id, [FromBody] JourneyDto resultDto)
        {
            var result = await _unitOfWork.Journeys.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            if (resultDto.Id == 0)
            {
                resultDto.Id = id;
            }
            if (resultDto.Id != id)
            {
                return BadRequest();
            }
            _mapper.Map(resultDto, result);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<JourneyDto>(result);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitOfWork.Journeys.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Journeys.Remove(result);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }


