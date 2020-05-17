using FutsalSystem.Models.DTO.User;
using FutsalSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FutsalSystem.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEntities()
        {
            try
            {
                IEnumerable<UserDTO> users = await _userService.GetEntities();
                return Ok(users);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetEntityById(int userId)
        {
            try
            {
                UserDTO user = await _userService.GetEntityById(userId);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody]UserDTO userDTO)
        {
            try
            {
                UserDTO createdUser = await _userService.CreateEntity(userDTO);
                return Ok(createdUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateEntity([FromBody] UserDTO userDTO, int userId)
        {
            try
            {
                if (userDTO.Id != userId)
                    throw new InvalidOperationException("Passed model id is not equal to request URL id.");

                await _userService.UpdateEntity(userDTO);
                return Ok(userDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteEntity(int userId)
        {
            try
            {
                await _userService.DeleteEntity(userId);
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
