using AutoMapper;
using FutsalSystem.Models;
using FutsalSystem.Models.DTO.Team;
using FutsalSystem.Repository.Interface;
using FutsalSystem.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FutsalSystem.SharedConfigurations.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FutsalSystem.Services
{
    public class TeamService : ITeamService
    {
        protected readonly IHubContext<ChatHub> _hubContext;
        protected readonly IRepository _repository;
        protected readonly IMapper _mapper;
        protected readonly IMatchService _matchService;
        private readonly IPlayerService _playerService;


        private readonly IConfiguration _conf;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        private string scheme;
        private string baseUrl;
        private string sharedVirtualPath;

        public TeamService(IRepository repository, IMapper mapper, IHttpContextAccessor cont, IConfiguration conf, IHostingEnvironment environment, IHubContext<ChatHub> hubContext, IMatchService matchService, IPlayerService playerService)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContext = cont;
            _conf = conf;
            _hubContext = hubContext;
            _matchService = matchService;
            _playerService = playerService;

            scheme = _httpContext.HttpContext.Request.Scheme.ToString(); // http : https
            baseUrl = _httpContext.HttpContext.Request.Host.Value; // localhost:<port>
            sharedVirtualPath = _conf.GetValue<string>("SharedImagesPath"); // app-images
            _hostingEnvironment = environment;
        }

        public string SaveImageToSharedDirectory(string imageBase64)
        {
            if (imageBase64.Length == 0)
            {
                return "";
            }
            string imageName = Guid.NewGuid().ToString() + ".png";
            string saveImagePath = _hostingEnvironment.ContentRootPath + "/Shared/Files/Images/" + imageName;
            byte[] bytes = Convert.FromBase64String(imageBase64);

            System.Drawing.Image bitmapImage;

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                bitmapImage = System.Drawing.Image.FromStream(ms);
                bitmapImage.Save(saveImagePath);

                if (File.Exists(_hostingEnvironment.ContentRootPath + "/Shared/Files/Images/" + imageName))
                    return scheme + "://" + baseUrl + "/" + sharedVirtualPath + "/" + imageName;
                return "";
            }

            
        }

        public async Task<IEnumerable<TeamDTO>> GetEntities()
        {
            IQueryable<Team> teams = await _repository.QueryAsync<Team>();
            return _mapper.Map<IEnumerable<TeamDTO>>(teams);
        }

        public async Task<IEnumerable<TeamDTO>> GetSortedEntities()
        {
            IQueryable<Team> teams = await _repository.QueryAsync<Team>();
            return _mapper.Map<IEnumerable<TeamDTO>>(teams).OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalsDifference);
        }

        public async Task<TeamDTO> GetEntityById(int teamId)
        {
            Team team = await _repository.QueryByIdAsync<Team>(teamId);
            if (team != null)
                return _mapper.Map<TeamDTO>(team);

            throw new System.InvalidOperationException($"Team with id {teamId} was not found.");
        }

        public async Task<TeamDTO> CreateEntity(TeamDTO teamDTO)
        {
            Team team = _mapper.Map<Team>(teamDTO);
            team.Id = 0;
            team.Points = team.GoalsFor = team.GoalsAgainst =
                team.MatchesPlayed = team.MatchesWon = team.MatchesDrawn = team.MatchesLost = 0;
            var imagePath = SaveImageToSharedDirectory(teamDTO.ImagePath);
            team.ImagePath = imagePath == "" ? "../../assets/team.svg" : imagePath;
            var createdTeam = await _repository.CreateAsync(team);
            var createdTeamDto = _mapper.Map<TeamDTO>(createdTeam);
            await _hubContext.Clients.All.SendAsync(ChatHubEnum.teamCreated.ToString(), createdTeamDto);
            return createdTeamDto;
        }

        public static bool IsBase64(string base64String)
        {
            if (base64String.Replace(" ", "").Length % 4 != 0)
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (FormatException exception)
            {
            }
            return false;
        }

        public async Task<TeamDTO> UpdateEntity(TeamDTO teamDTO)
        {
            if (!string.IsNullOrEmpty(teamDTO.ImagePath))
            {
                if (IsBase64(teamDTO.ImagePath))
                {
                    var imagePath = SaveImageToSharedDirectory(teamDTO.ImagePath);
                    teamDTO.ImagePath = imagePath;
                }
            }

            Team updatedTeam = _mapper.Map<Team>(teamDTO);
            await _repository.UpdateAsync(teamDTO.Id, updatedTeam);
            await _hubContext.Clients.All.SendAsync(ChatHubEnum.teamUpdated.ToString(), teamDTO);
            return teamDTO;
        }

        public async Task DeleteEntity(int teamId)
        {
            Team team = await _repository.QueryByIdAsync<Team>(teamId);
            if (team == null)
                throw new InvalidOperationException($"Team with id {teamId} not found.");

            var matches = await _matchService.GetEntities();
            var teamMatches = matches.Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId);
            var players = await _playerService.GetEntitiesByTeamId(teamId);


            foreach (var player in players)
            {
                await _playerService.DeleteEntity(player.Id);
            }

            foreach (var match in teamMatches)
            {
                await _matchService.DeleteEntity(match.Id);
            }
            

            await _repository.DeleteAsync<Team>(teamId);
        }
    }
}
