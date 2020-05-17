using AutoMapper;
using FutsalSystem.Models;
using FutsalSystem.Models.DTO.Announcement;
using FutsalSystem.Models.DTO.Match;
using FutsalSystem.Models.DTO.MatchEvent;
using FutsalSystem.Models.DTO.Player;
using FutsalSystem.Models.DTO.Team;
using FutsalSystem.Models.DTO.User;

namespace FutsalSystem.SharedConfigurations.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player, PlayerDTO>()
                .ReverseMap();
            CreateMap<Team, TeamDTO>()
                .ReverseMap();
            CreateMap<User, UserDTO>()
                .ReverseMap();
            CreateMap<Match, MatchDTO>()
                .ForMember(mDTO => mDTO.HomeTeam,
                    opt => opt.MapFrom(m => m.HomeTeam.Title))
                .ForMember(mDTO => mDTO.AwayTeam,
                    opt => opt.MapFrom(m => m.AwayTeam.Title))
                .ReverseMap();
            CreateMap<MatchEvent, MatchEventDTO>()
                .ReverseMap();
            CreateMap<Announcement, AnnouncementDTO>()
                .ReverseMap();
        }
    }
}
