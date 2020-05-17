using FutsalSystem.Controllers;
using FutsalSystem.Models.DTO.User;
using FutsalSystem.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FutsalTests
{
    public class UserControllerTests
    {
        [Fact]
        public void GetEntitiesTest()
        {
            var userService = new Mock<IUserService>();
            userService.Setup(mock => mock.GetEntities());

            var userController = new UserController(userService.Object);
            userController.GetEntities();

            userService.Verify(mock => mock.GetEntities(), Times.Once);
        }

        [Fact]
        public void GetEntityByIdTest()
        {
            var userService = new Mock<IUserService>();
            userService.Setup(mock => mock.GetEntityById(It.IsAny<int>()));

            var userController = new UserController(userService.Object);
            userController.GetEntityById(It.IsAny<int>());

            userService.Verify(mock => mock.GetEntityById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void CreateEntityTest()
        {
            var userService = new Mock<IUserService>();
            userService.Setup(mock => mock.CreateEntity(It.IsAny<UserDTO>()));

            var userController = new UserController(userService.Object);
            userController.CreateEntity(It.IsAny<UserDTO>());

            userService.Verify(mock => mock.CreateEntity(It.IsAny<UserDTO>()), Times.Once);
        }

        [Fact]
        public void UpdateEntityTest()
        {
            var userService = new Mock<IUserService>();
            userService.Setup(mock => mock.UpdateEntity(It.IsAny<UserDTO>()));

            var userController = new UserController(userService.Object);
            var tempUserDTO = new UserDTO
            {
                Id = 1,
                Username = "",
                Password = "",
                Email = "",
            };
            userController.UpdateEntity(tempUserDTO, tempUserDTO.Id);

            userService.Verify(mock => mock.UpdateEntity(It.IsAny<UserDTO>()), Times.Once);
        }

        [Fact]
        public void DeleteEntityTest()
        {
            var userService = new Mock<IUserService>();
            userService.Setup(mock => mock.DeleteEntity(It.IsAny<int>()));

            var userController = new UserController(userService.Object);
            userController.DeleteEntity(It.IsAny<int>());

            userService.Verify(mock => mock.DeleteEntity(It.IsAny<int>()), Times.Once);
        }
    }
}
