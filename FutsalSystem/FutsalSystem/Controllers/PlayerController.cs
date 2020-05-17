using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FutsalSystem.Models.DTO.Player;
using FutsalSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FutsalSystem.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost("uploadimage")]
        public async Task<IActionResult> PostImage([FromBody] string image)
        {
            try
            {
                var imagePath = _playerService.SaveImageToSharedDirectory(image);
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
                IEnumerable<PlayerDTO> players = await _playerService.GetEntities();
                return Ok(players);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("byTeam/{teamId}")]
        public async Task<IActionResult> GetEntitiesByTeamId(int teamId)
        {
            try
            {
                IEnumerable<PlayerDTO> players = await _playerService.GetEntitiesByTeamId(teamId);
                return Ok(players);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetEntityById(int playerId)
        {
            try
            {
                PlayerDTO player = await _playerService.GetEntityById(playerId);
                return Ok(player);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody]PlayerDTO playerDTO)
        {
            try
            {
                PlayerDTO createdPlayer = await _playerService.CreateEntity(playerDTO);
                return Ok(createdPlayer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{playerId}")]
        public async Task<IActionResult> UpdateEntity([FromBody] PlayerDTO playerDTO, int playerId)
        {
            try
            {
                if (playerDTO.Id != playerId)
                    throw new InvalidOperationException("Passed model id is not equal to request URL id.");

                await _playerService.UpdateEntity(playerDTO);
                return Ok(playerDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{playerId}")]
        public async Task<IActionResult> DeleteEntity(int playerId)
        {
            try
            {
                await _playerService.DeleteEntity(playerId);
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
