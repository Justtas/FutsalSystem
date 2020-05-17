using FutsalSystem.Models.DTO.Announcement;
using FutsalSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FutsalSystem.Controllers
{
    [Route("api/announcements")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;
        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEntities()
        {
            try
            {
                IEnumerable<AnnouncementDTO> announcements = await _announcementService.GetEntities();
                return Ok(announcements);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        [HttpGet("{announcementId}")]
        public async Task<IActionResult> GetEntityById(int announcementId)
        {
            try
            {
                AnnouncementDTO announcement = await _announcementService.GetEntityById(announcementId);
                return Ok(announcement);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntity([FromBody]AnnouncementDTO announcementDTO)
        {
            try
            {
                AnnouncementDTO createdAnnouncement = await _announcementService.CreateEntity(announcementDTO);
                return Ok(createdAnnouncement);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{announcementId}")]
        public async Task<IActionResult> UpdateEntity([FromBody] AnnouncementDTO announcementDTO, int announcementId)
        {
            try
            {
                if (announcementDTO.Id != announcementId)
                    throw new InvalidOperationException("Passed model id is not equal to request URL id.");

                await _announcementService.UpdateEntity(announcementDTO);
                return Ok(announcementDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{announcementId}")]
        public async Task<IActionResult> DeleteEntity(int announcementId)
        {
            try
            {
                await _announcementService.DeleteEntity(announcementId);
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
