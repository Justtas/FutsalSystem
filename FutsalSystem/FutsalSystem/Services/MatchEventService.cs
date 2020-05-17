using AutoMapper;
using FutsalSystem.Enumerators;
using FutsalSystem.Models;
using FutsalSystem.Models.DTO.MatchEvent;
using FutsalSystem.Repository.Interface;
using FutsalSystem.Services.Interfaces;
using FutsalSystem.SharedConfigurations.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FutsalSystem.Services
{
    public class MatchEventService : IMatchEventService
    {
        protected readonly IHubContext<ChatHub> _hubContext;
        protected readonly IRepository _repository;
        protected readonly IMapper _mapper;

        public MatchEventService(IRepository repository, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            _repository = repository;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<MatchEventDTO>> GetEntities(int matchId)
        {
            IQueryable<MatchEvent> matchEvents = await _repository.QueryAsync<MatchEvent>();
            matchEvents = matchEvents.Where(m => m.MatchId == matchId);
            var players = await _repository.QueryAsync<Player>();
            var matchEventsDTO = _mapper.Map<IEnumerable<MatchEventDTO>>(matchEvents).ToList().OrderBy(m => m.Minute);

            foreach (var matchEvent in matchEventsDTO)
            {
                var player = players.FirstOrDefault(p => p.Id == matchEvent.PlayerId);
                if (player == null) continue;
                var team = await _repository.QueryByIdAsync<Team>(Convert.ToInt32(player.TeamId));
                matchEvent.PlayerName = $"{player.FirstName} {player.LastName}";
                matchEvent.TeamName = team.Title;
            }

            return matchEventsDTO;
        }

        public async Task<MatchEventDTO> GetEntityById(int matchId, int matchEventId)
        {
            IQueryable<MatchEvent> matchEvents = await _repository.QueryAsync<MatchEvent>();
            var matchEvent = matchEvents.FirstOrDefault(m => m.MatchId == matchId && m.Id == matchEventId);
            var matchEventDTO = new MatchEventDTO();

            if (matchEvent == null)
                throw new System.InvalidOperationException($"A match event with id {matchEventId} was not found.");

            matchEventDTO = _mapper.Map<MatchEventDTO>(matchEvent);
            var player = await _repository.QueryByIdAsync<Player>(matchEventDTO.PlayerId);
            var team = await _repository.QueryByIdAsync<Team>(Convert.ToInt32(player.TeamId));
            matchEventDTO.PlayerName = $"{player.FirstName} {player.LastName}";
            matchEventDTO.TeamName = team.Title;
            return matchEventDTO;
        }

        public async Task<MatchEventDTO> CreateEntity(MatchEventDTO matchEventDTO)
        {
            MatchEvent matchEvent = _mapper.Map<MatchEvent>(matchEventDTO);
            matchEvent.Id = 0;
            var createdMatchEvent = await _repository.CreateAsyncWithoutSave(matchEvent);
            var players = await _repository.QueryAsync<Player>();
            var matchEventPlayer = players.FirstOrDefault(p => p.Id == matchEventDTO.PlayerId);
            var teams = await _repository.QueryAsync<Team>();
            var matches = await _repository.QueryAsync<Match>();
            var selectedMatch = matches.FirstOrDefault(m => m.Id == matchEventDTO.MatchId);
            var matchEventHomeTeam = teams.FirstOrDefault(t => t.Id == selectedMatch.HomeTeamId);
            var matchEventAwayTeam = teams.FirstOrDefault(t => t.Id == selectedMatch.AwayTeamId);
            switch (matchEventDTO.EventType)
            {
                case PlayerEvent.Goal:
                    {
                        if (matchEventPlayer.TeamId == matchEventHomeTeam.Id)
                        {
                            matchEventPlayer.Goals++;
                        }
                        else if (matchEventPlayer.TeamId == matchEventAwayTeam.Id)
                        {
                            matchEventPlayer.Goals++;
                        }
                        await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                        await _repository.SaveChangesAsync();
                        matchEventDTO.Id = createdMatchEvent.Id;
                        await _hubContext.Clients.All.SendAsync(ChatHubEnum.matchEventCreated.ToString(),
                            matchEventDTO);
                        break;
                    }
                case PlayerEvent.OwnGoal:
                    {
                        if (matchEventPlayer.TeamId == matchEventHomeTeam.Id)
                        {
                            matchEventPlayer.OwnGoals++;
                        }
                        else if (matchEventPlayer.TeamId == matchEventAwayTeam.Id)
                        {
                            matchEventPlayer.OwnGoals++;
                        }
                        await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                        await _repository.SaveChangesAsync();
                        matchEventDTO.Id = createdMatchEvent.Id;
                        await _hubContext.Clients.All.SendAsync(ChatHubEnum.matchEventCreated.ToString(), matchEventDTO);
                        break;
                    }
                case PlayerEvent.SixMetresKick:
                    if (matchEventPlayer.TeamId == matchEventHomeTeam.Id)
                    {
                        matchEventPlayer.Goals++;

                    }
                    else if (matchEventPlayer.TeamId == matchEventAwayTeam.Id)
                    {
                        matchEventPlayer.Goals++;

                    }
                    await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                    await _repository.SaveChangesAsync();
                    matchEventDTO.Id = createdMatchEvent.Id;
                    await _hubContext.Clients.All.SendAsync(ChatHubEnum.matchEventCreated.ToString(), matchEventDTO);
                    break;
                case PlayerEvent.TenMetresKick:
                    if (matchEventPlayer.TeamId == matchEventHomeTeam.Id)
                    {
                        matchEventPlayer.Goals++;
                    }
                    else if (matchEventPlayer.TeamId == matchEventAwayTeam.Id)
                    {
                        matchEventPlayer.Goals++;
                    }
                    await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                    await _repository.SaveChangesAsync();
                    matchEventDTO.Id = createdMatchEvent.Id;
                    await _hubContext.Clients.All.SendAsync(ChatHubEnum.matchEventCreated.ToString(), matchEventDTO);
                    break;
                case PlayerEvent.YellowCard:
                    matchEventPlayer.YellowCardsCount++;
                    await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                    await _repository.SaveChangesAsync();
                    matchEventDTO.Id = createdMatchEvent.Id;
                    await _hubContext.Clients.All.SendAsync(ChatHubEnum.matchEventCreated.ToString(), matchEventDTO);
                    break;
                case PlayerEvent.RedCard:
                    matchEventPlayer.RedCardsCount++;
                    await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                    await _repository.SaveChangesAsync();
                    matchEventDTO.Id = createdMatchEvent.Id;
                    await _hubContext.Clients.All.SendAsync(ChatHubEnum.matchEventCreated.ToString(), matchEventDTO);
                    break;
                case PlayerEvent.MissedFreeKick:
                    break;
            }

            return _mapper.Map<MatchEventDTO>(createdMatchEvent);
        }

        public async Task<MatchEventDTO> UpdateEntity(MatchEventDTO matchEventDTO, int matchId, int matchEventId)
        {
            MatchEvent updatedMatchEvent = _mapper.Map<MatchEvent>(matchEventDTO);
            updatedMatchEvent.MatchId = matchId;
            updatedMatchEvent.Id = matchEventId;
            await _repository.UpdateAsync(matchEventDTO.Id, updatedMatchEvent);
            matchEventDTO.Id = matchEventId;
            return matchEventDTO;
        }

        public async Task DeleteEntity(int matchEventId)
        {
            MatchEvent matchEvent = await _repository.QueryByIdAsync<MatchEvent>(matchEventId);
            if (matchEvent == null)
                throw new InvalidOperationException($"A match event with id {matchEventId} not found.");
            var players = await _repository.QueryAsync<Player>();
            var matchEventPlayer = players.FirstOrDefault(p => p.Id == matchEvent.PlayerId);
            var match = await _repository.QueryByIdAsync<Match>(matchEvent.MatchId);
            var matchEventDTO = _mapper.Map<MatchEventDTO>(matchEvent);


            switch (matchEvent.EventType)
            {
                case PlayerEvent.Goal:
                    {
                        matchEventPlayer.Goals--;
                        await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                        break;
                    }
                case PlayerEvent.OwnGoal:
                    {
                        matchEventPlayer.OwnGoals--;
                        await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                        break;
                    }
                case PlayerEvent.SixMetresKick:
                    matchEventPlayer.Goals--;
                    await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                    break;
                case PlayerEvent.TenMetresKick:
                    matchEventPlayer.Goals--;
                    await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                    break;
                case PlayerEvent.YellowCard:
                    matchEventPlayer.YellowCardsCount--;
                    await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                    break;
                case PlayerEvent.RedCard:
                    matchEventPlayer.RedCardsCount--;
                    await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                    break;
                case PlayerEvent.MissedFreeKick:
                    break;
            }

            await _repository.DeleteAsync<MatchEvent>(matchEventId);
        }
    }
}
