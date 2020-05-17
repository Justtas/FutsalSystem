using AutoMapper;
using FutsalSystem.Models;
using FutsalSystem.Models.DTO.Announcement;
using FutsalSystem.Repository.Interface;
using FutsalSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FutsalSystem.SharedConfigurations.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FutsalSystem.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        protected readonly IHubContext<ChatHub> _hubContext;
        protected readonly IRepository _repository;
        protected readonly IMapper _mapper;

        public AnnouncementService(IRepository repository, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            _repository = repository;
            _mapper = mapper;
            _hubContext = hubContext;

        }

        public async Task<IEnumerable<AnnouncementDTO>> GetEntities()
        {
            IQueryable<Announcement> announcements = await _repository.QueryAsync<Announcement>();
            return _mapper.Map<IEnumerable<AnnouncementDTO>>(announcements).OrderByDescending(a => a.CreationDate);
        }

        public async Task<AnnouncementDTO> GetEntityById(int announcementId)
        {
            Announcement announcement = await _repository.QueryByIdAsync<Announcement>(announcementId);
            if (announcement != null)
                return _mapper.Map<AnnouncementDTO>(announcement);

            throw new InvalidOperationException($"Announcement with id {announcementId} was not found.");
        }

        public async Task<AnnouncementDTO> CreateEntity(AnnouncementDTO announcementDTO)
        {
            Announcement announcement = _mapper.Map<Announcement>(announcementDTO);
            announcement.Id = 0;
            var dateToday = DateTime.Now;
            announcement.CreationDate = dateToday.ToString("yyyy-MM-dd HH:mm:ss");
            var createdAnnouncement = await _repository.CreateAsync(announcement);
            var createdAnnouncementDto = _mapper.Map<AnnouncementDTO>(createdAnnouncement);
            await _hubContext.Clients.All.SendAsync(ChatHubEnum.announcementCreated.ToString(), createdAnnouncementDto);
            return createdAnnouncementDto;
        }

        public async Task<AnnouncementDTO> UpdateEntity(AnnouncementDTO announcementDTO)
        {
            Announcement updatedAnnouncement = _mapper.Map<Announcement>(announcementDTO);
            await _repository.UpdateAsync(announcementDTO.Id, updatedAnnouncement);
            await _hubContext.Clients.All.SendAsync(ChatHubEnum.announcementUpdated.ToString(), announcementDTO);
            return announcementDTO;
        }

        public async Task DeleteEntity(int announcementId)
        {
            Announcement announcement = await _repository.QueryByIdAsync<Announcement>(announcementId);
            if (announcement == null)
                throw new InvalidOperationException($"Announcement with id {announcementId} not found.");

            await _repository.DeleteAsync<Announcement>(announcementId);
        }
    }
}
