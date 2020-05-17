using FutsalSystem.Controllers;
using FutsalSystem.Enumerators;
using FutsalSystem.Models.DTO.MatchEvent;
using FutsalSystem.Services.Interfaces;
using Moq;
using Xunit;

namespace FutsalTests
{
    public class MatchEventControllerTests
    {
        [Fact]
        public void GetEntitiesTest()
        {
            var matchEventService = new Mock<IMatchEventService>();
            matchEventService.Setup(mock => mock.GetEntities(It.IsAny<int>()));

            var matchEventController = new MatchEventController(matchEventService.Object);
            matchEventController.GetEntities(It.IsAny<int>());

            matchEventService.Verify(mock => mock.GetEntities(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetEntityByIdTest()
        {
            var matchEventService = new Mock<IMatchEventService>();
            matchEventService.Setup(mock => mock.GetEntityById(It.IsAny<int>(), It.IsAny<int>()));

            var matchEventController = new MatchEventController(matchEventService.Object);
            matchEventController.GetEntityById(It.IsAny<int>(), It.IsAny<int>());

            matchEventService.Verify(mock => mock.GetEntityById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void CreateEntityTest()
        {
            var matchEventService = new Mock<IMatchEventService>();
            matchEventService.Setup(mock => mock.CreateEntity(It.IsAny<MatchEventDTO>()));

            var matchController = new MatchEventController(matchEventService.Object);
            matchController.CreateEntity(It.IsAny<MatchEventDTO>());

            matchEventService.Verify(mock => mock.CreateEntity(It.IsAny<MatchEventDTO>()), Times.Once);
        }

        [Fact]
        public void UpdateEntityTest()
        {
            var matchEventService = new Mock<IMatchEventService>();
            matchEventService.Setup(mock => mock.UpdateEntity(It.IsAny<MatchEventDTO>(), It.IsAny<int>(), It.IsAny<int>()));

            var matchController = new MatchEventController(matchEventService.Object);
            var tempMatchEventDTO = new MatchEventDTO
            {
                Id = 1,
                MatchId = 1,
                Match = null,
                Minute = 1,
                PlayerId = 0,
                Player = null,
                PlayerName = "",
                TeamName = "",
                EventType = PlayerEvent.Goal
            };
            matchController.UpdateEntity(tempMatchEventDTO, tempMatchEventDTO.Id, It.IsAny<int>());

            matchEventService.Verify(mock => mock.UpdateEntity(It.IsAny<MatchEventDTO>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void DeleteEntityTest()
        {
            var matchEventService = new Mock<IMatchEventService>();
            matchEventService.Setup(mock => mock.DeleteEntity(It.IsAny<int>()));

            var matchController = new MatchEventController(matchEventService.Object);
            matchController.DeleteEntity(It.IsAny<int>());

            matchEventService.Verify(mock => mock.DeleteEntity(It.IsAny<int>()), Times.Once);
        }
    }
}
