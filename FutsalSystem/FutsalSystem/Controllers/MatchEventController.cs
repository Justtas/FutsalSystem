using FutsalSystem.Models.DTO.MatchEvent;
using FutsalSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FutsalSystem.Controllers
{
    [Route("api/matches/{matchId}/matchEvents")]
    [ApiController]
    public class MatchEventController : ControllerBase
    {
        private readonly IMatchEventService _matchEventService;

        public MatchEventController(IMatchEventService matchEventService)
        {
            _matchEventService = matchEventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEntities(int matchId)
        {
            try
            {
                IEnumerable<MatchEventDTO> matchEvents = await _matchEventService.GetEntities(matchId);
                return Ok(matchEvents);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [HttpGet("{matchEventId}")]
        public async Task<IActionResult> GetEntityById(int matchId, int matchEventId)
        {
            try
            {
                MatchEventDTO matchEvent = await _matchEventService.GetEntityById(matchId, matchEventId);
                return Ok(matchEvent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody]MatchEventDTO matchEventDTO)
        {
            try
            {
                MatchEventDTO createdMatchEvent = await _matchEventService.CreateEntity(matchEventDTO);
                return Ok(createdMatchEvent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{matchEventId}")]
        public async Task<IActionResult> UpdateEntity([FromBody] MatchEventDTO matchEventDTO, int matchId, int matchEventId)
        {
            try
            {
                if (matchEventDTO.Id != matchId)
                    throw new InvalidOperationException("Passed model id is not equal to request URL id.");

                await _matchEventService.UpdateEntity(matchEventDTO, matchId, matchEventId);
                return Ok(matchEventDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{matchEventId}")]
        public async Task<IActionResult> DeleteEntity(int matchEventId)
        {
            try
            {
                await _matchEventService.DeleteEntity(matchEventId);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
    }
}
