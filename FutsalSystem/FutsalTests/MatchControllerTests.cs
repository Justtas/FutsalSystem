using System;
using FutsalSystem.Controllers;
using FutsalSystem.Models.DTO.Match;
using FutsalSystem.Services.Interfaces;
using Moq;
using Xunit;

namespace FutsalTests
{
    public class MatchControllerTests
    {
        [Fact]
        public void GetEntitiesTest()
        {
            var matchService = new Mock<IMatchService>();
            matchService.Setup(mock => mock.GetEntities());

            var matchController = new MatchController(matchService.Object);
            matchController.GetEntities();

            matchService.Verify(mock => mock.GetEntities(), Times.Once);
        }

        [Fact]
        public void GetEntityByIdTest()
        {
            var matchService = new Mock<IMatchService>();
            matchService.Setup(mock => mock.GetEntityById(It.IsAny<int>()));

            var matchController = new MatchController(matchService.Object);
            matchController.GetEntityById(It.IsAny<int>());

            matchService.Verify(mock => mock.GetEntityById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void CreateEntityTest()
        {
            var matchService = new Mock<IMatchService>();
            matchService.Setup(mock => mock.CreateEntity(It.IsAny<MatchDTO>()));

            var matchController = new MatchController(matchService.Object);
            matchController.CreateEntity(It.IsAny<MatchDTO>());

            matchService.Verify(mock => mock.CreateEntity(It.IsAny<MatchDTO>()), Times.Once);
        }

        [Fact]
        public void UpdateEntityTest()
        {
            var matchService = new Mock<IMatchService>();
            matchService.Setup(mock => mock.UpdateEntity(It.IsAny<MatchDTO>()));


            var matchController = new MatchController(matchService.Object);

            var tempMatchDTO = new MatchDTO
            {
                Id = 1,
                HomeTeamId = 0,
                HomeTeam = "",
                AwayTeamId = 0,
                AwayTeam = "",
                MatchDate = DateTime.Now,
                HomeTeamFirstHalfScore = 0,
                AwayTeamFirstHalfScore = 0,
                HomeTeamScore = 0,
                AwayTeamScore = 0,
                IsFinished = false,
                MatchEvents = null
            };

            matchController.UpdateEntity(tempMatchDTO, tempMatchDTO.Id);

            matchService.Verify(mock => mock.UpdateEntity(It.IsAny<MatchDTO>()), Times.Once);
        }

        [Fact]
        public void UpdateEntityWithMatchEventsTest()
        {
            var matchService = new Mock<IMatchService>();
            matchService.Setup(mock => mock.UpdateEntityMatchEvents(It.IsAny<MatchDTO>()));

            var matchController = new MatchController(matchService.Object);
            var tempMatchDTO = new MatchDTO
            {
                Id = 1,
                HomeTeamId = 0,
                HomeTeam = "",
                AwayTeamId = 0,
                AwayTeam = "",
                MatchDate = DateTime.Now,
                HomeTeamFirstHalfScore = 0,
                AwayTeamFirstHalfScore = 0,
                HomeTeamScore = 0,
                AwayTeamScore = 0,
                IsFinished = false,
                MatchEvents = null
            };
            matchController.UpdateEntityWithMatchEvents(tempMatchDTO, tempMatchDTO.Id);

            matchService.Verify(mock => mock.UpdateEntityMatchEvents(It.IsAny<MatchDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteEntityTest()
        {
            var matchService = new Mock<IMatchService>();
            matchService.Setup(mock => mock.DeleteEntity(It.IsAny<int>()));

            var matchController = new MatchController(matchService.Object);
            matchController.DeleteEntity(It.IsAny<int>());

            matchService.Verify(mock => mock.DeleteEntity(It.IsAny<int>()), Times.Once);
        }
    }
}
