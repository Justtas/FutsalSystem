using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FutsalSystem.Enumerators;
using FutsalSystem.Models;
using FutsalSystem.Models.DTO.Match;
using FutsalSystem.Repository.Interface;
using FutsalSystem.Services.Interfaces;
using FutsalSystem.SharedConfigurations.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FutsalSystem.Services
{
    public class MatchService : IMatchService
    {
        protected readonly IHubContext<ChatHub> _hubContext;
        protected readonly IRepository _repository;
        protected readonly IMapper _mapper;

        public MatchService(IRepository repository, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            _repository = repository;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<MatchDTO>> GetEntities()
        {
            IQueryable<Match> matches = await _repository.QueryAsync<Match>();
            var matchesDTO = _mapper.Map<IEnumerable<MatchDTO>>(matches).OrderByDescending(m => m.MatchDate);
            foreach (var item in matchesDTO)
            {
                var homeTeam = await _repository.QueryByIdAsync<Team>(item.HomeTeamId);
                var awayTeam = await _repository.QueryByIdAsync<Team>(item.AwayTeamId);
                item.HomeTeam = homeTeam.Title;
                item.AwayTeam = awayTeam.Title;
            }
            return matchesDTO;
        }

        public async Task<MatchDTO> GetEntityById(int matchId)
        {
            Match match = await _repository.QueryByIdAsync<Match>(matchId);
            var matchEvents = _repository.QueryAsync<MatchEvent>().Result.ToListAsync().Result.Where(m => m.MatchId == matchId).ToList();
            if (match != null)
            {
                var matchDTO = _mapper.Map<MatchDTO>(match);
                return matchDTO;
            }

            throw new Exception($"Rungtynės su id {matchId} nerastos.");
        }

        public async Task<MatchDTO> CreateEntity(MatchDTO matchDTO)
        {
            Match match = _mapper.Map<Match>(matchDTO);
            match.Id = 0;
            match.IsFinished = false;
            match.HomeTeamFirstHalfScore = match.HomeTeamScore = match.AwayTeamFirstHalfScore = match.AwayTeamScore = 0;
            match.HomeTeam = await _repository.QueryByIdAsync<Team>(match.HomeTeamId);
            match.AwayTeam = await _repository.QueryByIdAsync<Team>(match.AwayTeamId);
            var createdMatch = await _repository.CreateAsync(match);
            var createdMatchDto = _mapper.Map<MatchDTO>(createdMatch);
            await _hubContext.Clients.All.SendAsync(ChatHubEnum.matchCreated.ToString(), createdMatchDto);
            return createdMatchDto;
        }

        public async Task<MatchDTO> UpdateEntity(MatchDTO matchDTO)
        {
            Match updatedMatch = _mapper.Map<Match>(matchDTO);
            await _repository.UpdateAsync(matchDTO.Id, updatedMatch);
            await _hubContext.Clients.All.SendAsync(ChatHubEnum.matchUpdated.ToString(), matchDTO);
            return matchDTO;
        }

        public async Task<MatchDTO> UpdateEntityMatchEvents(MatchDTO matchDTO)
        {
            var createdMatchEvents = new List<MatchEvent>();
            foreach (var matchEvent in matchDTO.MatchEvents)
            {
                await _repository.CreateAsyncWithoutSave(matchEvent);
                createdMatchEvents.Add(matchEvent);
                var matchEventPlayer = await _repository.QueryByIdAsync<Player>(matchEvent.PlayerId);
                switch (matchEvent.EventType)
                {
                    case PlayerEvent.Goal:
                        {

                            matchEventPlayer.Goals++;
                            await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                            break;
                        }
                    case PlayerEvent.OwnGoal:
                        {
                            matchEventPlayer.OwnGoals++;

                            await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                            break;
                        }
                    case PlayerEvent.SixMetresKick:
                        matchEventPlayer.Goals++;
                        await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                        break;
                    case PlayerEvent.TenMetresKick:
                        matchEventPlayer.Goals++;
                        await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                        break;
                    case PlayerEvent.YellowCard:
                        matchEventPlayer.YellowCardsCount++;
                        await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                        break;
                    case PlayerEvent.RedCard:
                        matchEventPlayer.RedCardsCount++;
                        await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                        break;
                    case PlayerEvent.MissedFreeKick:
                        break;
                }
            }

            if (!matchDTO.IsFinished)
            {
                var selectedMatch = new Match();
                var updatedMatch = new Match();

                var homeTeam = await _repository.QueryByIdAsync<Team>(matchDTO.HomeTeamId);
                var awayTeam = await _repository.QueryByIdAsync<Team>(matchDTO.AwayTeamId);

                updatedMatch.HomeTeam = homeTeam;
                updatedMatch.AwayTeam = awayTeam;

                updatedMatch.Id = matchDTO.Id;
                updatedMatch.MatchDate = matchDTO.MatchDate;
                updatedMatch.HomeTeamId = matchDTO.HomeTeamId;
                updatedMatch.AwayTeamId = matchDTO.AwayTeamId;


                int homeTeamScore = 0,
                    homeTeamGoalsFor = 0,
                    homeTeamGoalsAgainst = 0,
                    homeTeamMatchesPlayed = 0,
                    homeTeamMatchesWon = 0,
                    homeTeamMatchesDrawn = 0,
                    homeTeamMatchesLost = 0,
                    homeTeamPointsGained = 0;

                int awayTeamScore = 0,
                    awayTeamGoalsFor = 0,
                    awayTeamGoalsAgainst = 0,
                    awayTeamMatchesPlayed = 0,
                    awayTeamMatchesWon = 0,
                    awayTeamMatchesDrawn = 0,
                    awayTeamMatchesLost = 0,
                    awayTeamPointsGained = 0;

                foreach (var matchEvent in matchDTO.MatchEvents)
                {
                    var selectedPlayer = await _repository.QueryByIdAsync<Player>(matchEvent.PlayerId);
                    switch (matchEvent.EventType)
                    {
                        case PlayerEvent.Goal:
                            {
                                if (selectedPlayer.TeamId == homeTeam.Id)
                                {
                                    homeTeamGoalsFor++;
                                    awayTeamGoalsAgainst++;
                                    homeTeamScore++;
                                }
                                else if (selectedPlayer.TeamId == awayTeam.Id)
                                {
                                    awayTeamGoalsFor++;
                                    homeTeamGoalsAgainst++;
                                    awayTeamScore++;
                                }
                                break;
                            }
                        case PlayerEvent.OwnGoal:
                            {
                                if (selectedPlayer.TeamId == homeTeam.Id)
                                {
                                    awayTeamGoalsFor++;
                                    homeTeamGoalsAgainst++;
                                    awayTeamScore++;
                                }
                                else if (selectedPlayer.TeamId == awayTeam.Id)
                                {
                                    homeTeamGoalsFor++;
                                    awayTeamGoalsAgainst++;
                                    homeTeamScore++;
                                }
                                break;
                            }
                        case PlayerEvent.SixMetresKick:
                            if (selectedPlayer.TeamId == homeTeam.Id)
                            {
                                homeTeamGoalsFor++;
                                awayTeamGoalsAgainst++;
                                homeTeamScore++;
                            }
                            else if (selectedPlayer.TeamId == awayTeam.Id)
                            {
                                awayTeamGoalsFor++;
                                homeTeamGoalsAgainst++;
                                awayTeamScore++;
                            }
                            break;
                        case PlayerEvent.TenMetresKick:
                            if (selectedPlayer.TeamId == homeTeam.Id)
                            {
                                homeTeamGoalsFor++;
                                awayTeamGoalsAgainst++;
                                homeTeamScore++;
                            }
                            else if (selectedPlayer.TeamId == awayTeam.Id)
                            {
                                awayTeamGoalsFor++;
                                homeTeamGoalsAgainst++;
                                awayTeamScore++;
                            }
                            break;
                        case PlayerEvent.YellowCard:
                            break;
                        case PlayerEvent.RedCard:
                            break;
                        case PlayerEvent.MissedFreeKick:
                            break;
                    }
                }


                selectedMatch.HomeTeamScore = homeTeamScore;
                selectedMatch.AwayTeamScore = awayTeamScore;

                if (homeTeamScore > awayTeamScore)
                {
                    homeTeamMatchesPlayed = 1;
                    homeTeamMatchesWon = 1;
                    homeTeamPointsGained = 3;
                    awayTeamMatchesPlayed = 1;
                    awayTeamMatchesLost = 1;
                    awayTeamPointsGained = 0;
                }
                else if (homeTeamScore == awayTeamScore) 
                {
                    homeTeamMatchesPlayed = 1;
                    homeTeamMatchesDrawn = 1;
                    homeTeamPointsGained = 1;
                    awayTeamMatchesPlayed = 1;
                    awayTeamMatchesDrawn = 1;
                    awayTeamPointsGained = 1;
                }
                else if (homeTeamScore < awayTeamScore) 
                {
                    homeTeamMatchesPlayed = 1;
                    homeTeamMatchesLost = 1;
                    homeTeamPointsGained = 0;
                    awayTeamMatchesPlayed = 1;
                    awayTeamMatchesWon = 1;
                    awayTeamPointsGained = 3;
                }



                homeTeam.MatchesPlayed += homeTeamMatchesPlayed;
                homeTeam.MatchesDrawn += homeTeamMatchesDrawn;
                homeTeam.MatchesLost += homeTeamMatchesLost;
                homeTeam.MatchesWon += homeTeamMatchesWon;
                homeTeam.Points += homeTeamPointsGained;
                homeTeam.GoalsFor += homeTeamGoalsFor;
                homeTeam.GoalsAgainst += homeTeamGoalsAgainst;

                awayTeam.MatchesPlayed += awayTeamMatchesPlayed;
                awayTeam.MatchesDrawn += awayTeamMatchesDrawn;
                awayTeam.MatchesLost += awayTeamMatchesLost;
                awayTeam.MatchesWon += awayTeamMatchesWon;
                awayTeam.Points += awayTeamPointsGained;
                awayTeam.GoalsFor += awayTeamGoalsFor;
                awayTeam.GoalsAgainst += awayTeamGoalsAgainst;




                updatedMatch.AwayTeamScore = awayTeamScore;
                updatedMatch.HomeTeamScore = homeTeamScore;
                updatedMatch.IsFinished = true;
                matchDTO.AwayTeamScore = awayTeamScore;
                matchDTO.HomeTeamScore = homeTeamScore;
                matchDTO.IsFinished = true;

                var players = await _repository.QueryAsync<Player>();
                var matchPlayers = players.Where(p => p.TeamId == homeTeam.Id || p.TeamId == awayTeam.Id);

                foreach (var player in matchPlayers)
                {
                    player.MatchesPlayed++;
                    await _repository.UpdateAsyncWithoutSave(player.Id, player);
                }

                await _repository.UpdateAsyncWithoutSave(homeTeam.Id, homeTeam);
                await _repository.UpdateAsyncWithoutSave(awayTeam.Id, awayTeam);
                await _repository.UpdateAsyncWithoutSave(selectedMatch.Id, updatedMatch);

                await _repository.SaveChangesAsync();
            }
            matchDTO.MatchEvents = new List<MatchEvent>();
            await _hubContext.Clients.All.SendAsync(ChatHubEnum.matchEnded.ToString(), matchDTO);
            return matchDTO;
        }

        public async Task DeleteEntity(int matchId)
        {
            Match match = await _repository.QueryByIdAsync<Match>(matchId);
            var matchEvents = _repository.QueryAsync<MatchEvent>().Result.ToListAsync().Result.Where(m => m.MatchId == match.Id).ToList();
            match.MatchEvents = matchEvents;
            if (match == null)
                throw new InvalidOperationException($"A match with id {matchId} not found.");
            if (!match.IsFinished)
            {
                await _repository.DeleteAsync<Match>(matchId);
            }
            else
            {
                ;
                var homeTeam = await _repository.QueryByIdAsync<Team>(match.HomeTeamId);
                var awayTeam = await _repository.QueryByIdAsync<Team>(match.AwayTeamId);

                int homeTeamScore = 0,
                    homeTeamGoalsFor = 0,
                    homeTeamGoalsAgainst = 0,
                    homeTeamMatchesPlayed = 0,
                    homeTeamMatchesWon = 0,
                    homeTeamMatchesDrawn = 0,
                    homeTeamMatchesLost = 0,
                    homeTeamPointsGained = 0;

                int awayTeamScore = 0,
                    awayTeamGoalsFor = 0,
                    awayTeamGoalsAgainst = 0,
                    awayTeamMatchesPlayed = 0,
                    awayTeamMatchesWon = 0,
                    awayTeamMatchesDrawn = 0,
                    awayTeamMatchesLost = 0,
                    awayTeamPointsGained = 0;
                foreach (var matchEvent in match.MatchEvents)
                {
                    var matchEventPlayer = await _repository.QueryByIdAsync<Player>(matchEvent.PlayerId);
                    switch (matchEvent.EventType)
                    {
                        case PlayerEvent.Goal:
                            {

                                matchEventPlayer.Goals--;
                                if (matchEventPlayer.TeamId == homeTeam.Id)
                                {
                                    homeTeamGoalsFor--;
                                    awayTeamGoalsAgainst--;
                                    homeTeamScore--;
                                }
                                else if (matchEventPlayer.TeamId == awayTeam.Id)
                                {
                                    awayTeamGoalsFor--;
                                    homeTeamGoalsAgainst--;
                                    awayTeamScore--;
                                }
                                await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                                break;
                            }
                        case PlayerEvent.OwnGoal:
                            {
                                matchEventPlayer.OwnGoals--;
                                if (matchEventPlayer.TeamId == homeTeam.Id)
                                {
                                    awayTeamGoalsFor--;
                                    homeTeamGoalsAgainst--;
                                    awayTeamScore--;
                                }
                                else if (matchEventPlayer.TeamId == awayTeam.Id)
                                {
                                    homeTeamGoalsFor--;
                                    awayTeamGoalsAgainst--;
                                    homeTeamScore--;
                                }
                                await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                                break;
                            }
                        case PlayerEvent.SixMetresKick:
                            matchEventPlayer.Goals--;
                            if (matchEventPlayer.TeamId == homeTeam.Id)
                            {
                                homeTeamGoalsFor--;
                                awayTeamGoalsAgainst--;
                                homeTeamScore--;
                            }
                            else if (matchEventPlayer.TeamId == awayTeam.Id)
                            {
                                awayTeamGoalsFor--;
                                homeTeamGoalsAgainst--;
                                awayTeamScore--;
                            }
                            await _repository.UpdateAsyncWithoutSave(matchEventPlayer.Id, matchEventPlayer);
                            break;
                        case PlayerEvent.TenMetresKick:
                            matchEventPlayer.Goals--;
                            if (matchEventPlayer.TeamId == homeTeam.Id)
                            {
                                homeTeamGoalsFor--;
                                awayTeamGoalsAgainst--;
                                homeTeamScore--;
                            }
                            else if (matchEventPlayer.TeamId == awayTeam.Id)
                            {
                                awayTeamGoalsFor--;
                                homeTeamGoalsAgainst--;
                                awayTeamScore--;
                            }
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

                }

                if (Math.Abs(homeTeamScore) > Math.Abs(awayTeamScore))
                {
                    homeTeamMatchesPlayed = -1;
                    homeTeamMatchesWon = -1;
                    homeTeamPointsGained = -3;
                    awayTeamMatchesPlayed = -1;
                    awayTeamMatchesLost = -1;
                    awayTeamPointsGained = 0;
                }
                else if (Math.Abs(homeTeamScore) == Math.Abs(awayTeamScore))
                {
                    homeTeamMatchesPlayed = -1;
                    homeTeamMatchesDrawn = -1;
                    homeTeamPointsGained = -1;
                    awayTeamMatchesPlayed = -1;
                    awayTeamMatchesDrawn = -1;
                    awayTeamPointsGained = -1;
                }
                else if (Math.Abs(homeTeamScore) < Math.Abs(awayTeamScore)) 
                {
                    homeTeamMatchesPlayed = -1;
                    homeTeamMatchesLost = -1;
                    homeTeamPointsGained = 0;
                    awayTeamMatchesPlayed = -1;
                    awayTeamMatchesWon = -1;
                    awayTeamPointsGained = -3;
                }


                homeTeam.MatchesPlayed += homeTeamMatchesPlayed;
                homeTeam.MatchesDrawn += homeTeamMatchesDrawn;
                homeTeam.MatchesLost += homeTeamMatchesLost;
                homeTeam.MatchesWon += homeTeamMatchesWon;
                homeTeam.Points += homeTeamPointsGained;
                homeTeam.GoalsFor += homeTeamGoalsFor;
                homeTeam.GoalsAgainst += homeTeamGoalsAgainst;

                awayTeam.MatchesPlayed += awayTeamMatchesPlayed;
                awayTeam.MatchesDrawn += awayTeamMatchesDrawn;
                awayTeam.MatchesLost += awayTeamMatchesLost;
                awayTeam.MatchesWon += awayTeamMatchesWon;
                awayTeam.Points += awayTeamPointsGained;
                awayTeam.GoalsFor += awayTeamGoalsFor;
                awayTeam.GoalsAgainst += awayTeamGoalsAgainst;

                await _repository.UpdateAsyncWithoutSave(homeTeam.Id, homeTeam);
                await _repository.UpdateAsyncWithoutSave(awayTeam.Id, awayTeam);

            }
            await _repository.DeleteAsyncWithoutSave<Match>(matchId);
            await _repository.SaveChangesAsync();
        }
    }
}
