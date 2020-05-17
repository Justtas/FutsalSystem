using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FutsalSystem.Models.DTO.Team;
using FutsalSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FutsalSystem.Controllers
{
    [Route("api/teams")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("uploadimage")]
        public async Task<IActionResult> PostImage([FromBody] string image)
        {
            try
            {
                var imagePath = _teamService.SaveImageToSharedDirectory(image);
                return Ok(imagePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEntities()
        {
            try
            {
                IEnumerable<TeamDTO> teams = await _teamService.GetEntities();
                return Ok(teams);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [HttpGet("sorted")]
        public async Task<IActionResult> GetSortedEntities()
        {
            try
            {
                IEnumerable<TeamDTO> teams = await _teamService.GetSortedEntities();
                return Ok(teams);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetEntityById(int teamId)
        {
            try
            {
                TeamDTO team = await _teamService.GetEntityById(teamId);
                return Ok(team);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody]TeamDTO teamDTO)
        {
            try
            {
                TeamDTO createdTeam = await _teamService.CreateEntity(teamDTO);
                return Ok(createdTeam);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{teamId}")]
        public async Task<IActionResult> UpdateEntity([FromBody] TeamDTO teamDTO, int teamId)
        {
            try
            {
                if (teamDTO.Id != teamId)
                    throw new InvalidOperationException("Passed model id is not equal to request URL id.");

                await _teamService.UpdateEntity(teamDTO);
                return Ok(teamDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{teamId}")]
        public async Task<IActionResult> DeleteEntity(int teamId)
        {
            try
            {
                await _teamService.DeleteEntity(teamId);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }

    }
}
