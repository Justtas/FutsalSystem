using FutsalSystem.Controllers;
using FutsalSystem.Models.DTO.Player;
using FutsalSystem.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FutsalTests
{
    public class PlayerControllerTests
    {
        [Fact]
        public void GetEntitiesTest()
        {
            var playerService = new Mock<IPlayerService>();
            playerService.Setup(mock => mock.GetEntities());

            var playerController = new PlayerController(playerService.Object);
            playerController.GetEntities();

            playerService.Verify(mock => mock.GetEntities(), Times.Once);
        }

        [Fact]
        public void GetEntitiesByTeamIdTest()
        {
            var playerService = new Mock<IPlayerService>();
            playerService.Setup(mock => mock.GetEntitiesByTeamId(It.IsAny<int>()));

            var playerController = new PlayerController(playerService.Object);
            playerController.GetEntitiesByTeamId(It.IsAny<int>());

            playerService.Verify(mock => mock.GetEntitiesByTeamId(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetEntityByIdTest()
        {
            var playerService = new Mock<IPlayerService>();
            playerService.Setup(mock => mock.GetEntityById(It.IsAny<int>()));

            var playerController = new PlayerController(playerService.Object);
            playerController.GetEntityById(It.IsAny<int>());

            playerService.Verify(mock => mock.GetEntityById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void CreateEntityTest()
        {
            var playerService = new Mock<IPlayerService>();
            playerService.Setup(mock => mock.CreateEntity(It.IsAny<PlayerDTO>()));

            var playerController = new PlayerController(playerService.Object);
            playerController.CreateEntity(It.IsAny<PlayerDTO>());

            playerService.Verify(mock => mock.CreateEntity(It.IsAny<PlayerDTO>()), Times.Once);
        }

        [Fact]
        public void UpdateEntityTest()
        {
            var playerService = new Mock<IPlayerService>();
            playerService.Setup(mock => mock.UpdateEntity(It.IsAny<PlayerDTO>()));

            var playerController = new PlayerController(playerService.Object);
            var tempPlayerDTO = new PlayerDTO
            {
                Id = 1,
                FirstName = "",
                LastName = "",
                DateOfBirth = DateTime.Now,
                YellowCardsCount = 0,
                RedCardsCount = 0,
                MatchesPlayed = 0,
                ImagePath = "",
                Goals = 0,
                OwnGoals = 0,
                Number = 0,
                TeamId = 0,
                TeamName = ""
            };
            playerController.UpdateEntity(tempPlayerDTO, tempPlayerDTO.Id);

            playerService.Verify(mock => mock.UpdateEntity(It.IsAny<PlayerDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteEntityTest()
        {
            var playerService = new Mock<IPlayerService>();
            playerService.Setup(mock => mock.DeleteEntity(It.IsAny<int>()));

            var playerController = new PlayerController(playerService.Object);
            playerController.DeleteEntity(It.IsAny<int>());

            playerService.Verify(mock => mock.DeleteEntity(It.IsAny<int>()), Times.Once);
        }
    }
}
