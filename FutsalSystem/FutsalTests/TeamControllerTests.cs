using FutsalSystem.Controllers;
using FutsalSystem.Models.DTO.Team;
using FutsalSystem.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FutsalTests
{
    public class TeamControllerTests
    {
        [Fact]
        public void GetEntitiesTest()
        {
            var teamService = new Mock<ITeamService>();
            teamService.Setup(mock => mock.GetEntities());

            var teamController = new TeamController(teamService.Object);
            teamController.GetEntities();

            teamService.Verify(mock => mock.GetEntities(), Times.Once);
        }

        [Fact]
        public void GetSortedEntitiesTest()
        {
            var teamService = new Mock<ITeamService>();
            teamService.Setup(mock => mock.GetSortedEntities());

            var teamController = new TeamController(teamService.Object);
            teamController.GetSortedEntities();

            teamService.Verify(mock => mock.GetSortedEntities(), Times.Once);
        }

        [Fact]
        public void GetEntityByIdTest()
        {
            var teamService = new Mock<ITeamService>();
            teamService.Setup(mock => mock.GetEntityById(It.IsAny<int>()));

            var teamController = new TeamController(teamService.Object);
            teamController.GetEntityById(It.IsAny<int>());

            teamService.Verify(mock => mock.GetEntityById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void CreateEntityTest()
        {
            var teamService = new Mock<ITeamService>();
            teamService.Setup(mock => mock.CreateEntity(It.IsAny<TeamDTO>()));

            var teamController = new TeamController(teamService.Object);
            teamController.CreateEntity(It.IsAny<TeamDTO>());

            teamService.Verify(mock => mock.CreateEntity(It.IsAny<TeamDTO>()), Times.Once);
        }

        [Fact]
        public void UpdateEntityTest()
        {
            var teamService = new Mock<ITeamService>();
            teamService.Setup(mock => mock.UpdateEntity(It.IsAny<TeamDTO>()));

            var teamController = new TeamController(teamService.Object);
            var tempTeamDTO = new TeamDTO
            {
                Id = 1,
                Title = "",
                Points = 0,
                GoalsFor = 0,
                GoalsAgainst = 0,
                MatchesPlayed = 0,
                MatchesWon = 0,
                MatchesDrawn = 0,
                MatchesLost = 0,
                ImagePath = ""
            };
            teamController.UpdateEntity(tempTeamDTO, tempTeamDTO.Id);

            teamService.Verify(mock => mock.UpdateEntity(It.IsAny<TeamDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteEntityTest()
        {
            var teamService = new Mock<ITeamService>();
            teamService.Setup(mock => mock.DeleteEntity(It.IsAny<int>()));

            var teamController = new TeamController(teamService.Object);
            teamController.DeleteEntity(It.IsAny<int>());

            teamService.Verify(mock => mock.DeleteEntity(It.IsAny<int>()), Times.Once);
        }
    }
}
