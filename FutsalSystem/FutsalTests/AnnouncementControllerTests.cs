using FutsalSystem.Controllers;
using FutsalSystem.Models.DTO.Announcement;
using FutsalSystem.Services.Interfaces;
using Moq;
using Xunit;

namespace FutsalTests
{
    public class AnnouncementControllerTests
    {
        [Fact]
        public void GetEntitiesTest()
        {
            var announcementService = new Mock<IAnnouncementService>();
            announcementService.Setup(mock => mock.GetEntities());

            var announcementController = new AnnouncementController(announcementService.Object);
            announcementController.GetEntities();

            announcementService.Verify(mock => mock.GetEntities(), Times.Once);
        }

        [Fact]
        public void GetEntityByIdTest()
        {
            var announcementService = new Mock<IAnnouncementService>();
            announcementService.Setup(mock => mock.GetEntityById(It.IsAny<int>()));

            var announcementController = new AnnouncementController(announcementService.Object);
            announcementController.GetEntityById(It.IsAny<int>());

            announcementService.Verify(mock => mock.GetEntityById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void CreateEntityTest()
        {
            var announcementService = new Mock<IAnnouncementService>();
            announcementService.Setup(mock => mock.CreateEntity(It.IsAny<AnnouncementDTO>()));

            var announcementController = new AnnouncementController(announcementService.Object);
            announcementController.CreateEntity(It.IsAny<AnnouncementDTO>());

            announcementService.Verify(mock => mock.CreateEntity(It.IsAny<AnnouncementDTO>()), Times.Once);
        }

        [Fact]
        public void UpdateEntityTest()
        {
            var announcementService = new Mock<IAnnouncementService>();
            announcementService.Setup(mock => mock.UpdateEntity(It.IsAny<AnnouncementDTO>()));

            var announcementController = new AnnouncementController(announcementService.Object);
            var tempAnnouncementDTO = new AnnouncementDTO
            {
                Id = 1,
                Title = "",
                CreationDate = "",
                Message = "",
            };
            announcementController.UpdateEntity(tempAnnouncementDTO, tempAnnouncementDTO.Id);

            announcementService.Verify(mock => mock.UpdateEntity(It.IsAny<AnnouncementDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteEntityTest()
        {
            var announcementService = new Mock<IAnnouncementService>();
            announcementService.Setup(mock => mock.DeleteEntity(It.IsAny<int>()));

            var announcementController = new AnnouncementController(announcementService.Object);
            announcementController.DeleteEntity(It.IsAny<int>());

            announcementService.Verify(mock => mock.DeleteEntity(It.IsAny<int>()), Times.Once);
        }
    }
}
