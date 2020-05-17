using System;
using FutsalSystem.Models;
using FutsalSystem.Repository.Interface;
using FutsalSystem.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FutsalSystem.Models.DTO.Player;
using FutsalSystem.SharedConfigurations.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace FutsalSystem.Services
{
    public class PlayerService : IPlayerService
    {
        protected readonly IHubContext<ChatHub> _hubContext;
        protected readonly IRepository _repository;
        protected readonly IMapper _mapper;

        private readonly IConfiguration _conf;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        private string scheme;
        private string baseUrl;
        private string sharedVirtualPath;

        public PlayerService(IRepository repository, IMapper mapper, IHttpContextAccessor cont, IConfiguration conf, IHostingEnvironment environment, IHubContext<ChatHub> hubContext)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContext = cont;
            _conf = conf;
            _hubContext = hubContext;

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

        public async Task<IEnumerable<PlayerDTO>> GetEntities()
        {
            IQueryable<Player> players = await _repository.QueryAsync<Player>();
            return _mapper.Map<IEnumerable<PlayerDTO>>(players);
        }

        public async Task<PlayerDTO> GetEntityById(int playerId)
        {
            Player player = await _repository.QueryByIdAsync<Player>(playerId);
            Team team = new Team();
            if (player != null)
            {
                if (player.TeamId != null)
                {
                    var teamId = Convert.ToInt32(player.TeamId);
                    team = await _repository.QueryByIdAsync<Team>(teamId);
                }

                var playerDTO = _mapper.Map<PlayerDTO>(player);
                playerDTO.TeamName = team.Title;
                return playerDTO;
            }

            throw new System.InvalidOperationException($"Žaidėjas su id {playerId} neegzistuoja.");
        }

        public async Task<IEnumerable<PlayerDTO>> GetEntitiesByTeamId(int teamId)
        {
            var allPlayers = await _repository.QueryAsync<Player>();
            var teamPlayers = allPlayers.Where(x => x.TeamId == teamId);
            var teams = await _repository.QueryAsync<Team>();
            var allPlayersDTO = _mapper.Map<IEnumerable<PlayerDTO>>(teamPlayers);
            foreach (var player in allPlayersDTO)
            {
                var selectedTeam = teams.FirstOrDefault(t => t.Id == player.TeamId);
                if (selectedTeam != null) player.TeamName = selectedTeam.Title;
            }

            return allPlayersDTO;
        }

        public async Task<PlayerDTO> CreateEntity(PlayerDTO playerDTO)
        {
            Player player = _mapper.Map<Player>(playerDTO);
            player.Id = 0;
            player.YellowCardsCount = player.RedCardsCount = player.Goals = player.OwnGoals = player.MatchesPlayed = 0;
            var imagePath = SaveImageToSharedDirectory(playerDTO.ImagePath);
            player.ImagePath = imagePath == "" ? "../../assets/player.svg" : imagePath;
            if (player.Number == null)
                player.Number = 0;
            var createdPlayer = await _repository.CreateAsync(player);
            var createdPlayerDto = _mapper.Map<PlayerDTO>(createdPlayer);
            await _hubContext.Clients.All.SendAsync(ChatHubEnum.playerCreated.ToString(), createdPlayerDto);
            return createdPlayerDto;
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

        public async Task<PlayerDTO> UpdateEntity(PlayerDTO playerDTO)
        {
            if (!string.IsNullOrEmpty(playerDTO.ImagePath))
            {
                if (IsBase64(playerDTO.ImagePath))
                {
                    var imagePath = SaveImageToSharedDirectory(playerDTO.ImagePath);
                    playerDTO.ImagePath = imagePath;
                }
            }

            Player updatedPlayer = _mapper.Map<Player>(playerDTO);
            await _repository.UpdateAsync(playerDTO.Id, updatedPlayer);
            await _hubContext.Clients.All.SendAsync(ChatHubEnum.playerUpdated.ToString(), playerDTO);
            return playerDTO;
        }

        public async Task DeleteEntity(int playerId)
        {
            Player player = await _repository.QueryByIdAsync<Player>(playerId);
            if (player == null)
                throw new InvalidOperationException($"Player with id {playerId} not found.");

            await _repository.DeleteAsync<Player>(playerId);
        }
    }
}
