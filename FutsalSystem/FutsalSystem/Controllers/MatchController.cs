using FutsalSystem.Models.DTO.Match;
using FutsalSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FutsalSystem.Controllers
{
    [Route("api/matches")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEntities()
        {
            try
            {
                IEnumerable<MatchDTO> matches = await _matchService.GetEntities();
                return Ok(matches);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [HttpGet("{matchId}")]
        public async Task<IActionResult> GetEntityById(int matchId)
        {
            try
            {
                MatchDTO match = await _matchService.GetEntityById(matchId);
                return Ok(match);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody]MatchDTO matchDTO)
        {
            try
            {
                MatchDTO createdMatch = await _matchService.CreateEntity(matchDTO);
                return Ok(createdMatch);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{matchId}")]
        public async Task<IActionResult> UpdateEntity([FromBody] MatchDTO matchDTO, int matchId)
        {
            try
            {
                if (matchDTO.Id != matchId)
                    throw new InvalidOperationException("Passed model id is not equal to request URL id.");

                await _matchService.UpdateEntity(matchDTO);
                return Ok(matchDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{matchId}/matchEventsUpdate")]
        public async Task<IActionResult> UpdateEntityWithMatchEvents([FromBody] MatchDTO matchDTO, int matchId)
        {
            try
            {
                if (matchDTO.Id != matchId)
                    throw new InvalidOperationException("Passed model id is not equal to request URL id.");

                await _matchService.UpdateEntityMatchEvents(matchDTO);
                return Ok(matchDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{matchId}")]
        public async Task<IActionResult> DeleteEntity(int matchId)
        {
            try
            {
                await _matchService.DeleteEntity(matchId);
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
