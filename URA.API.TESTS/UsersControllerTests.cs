using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URA.API.Controllers;
using URA.API.Domain.Models;
using URA.API.Domain.Services;

namespace URA.API.TESTS
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUsersService> _serviceStub;
        private UsersController _controller;
        private IEnumerable<User> _users;


        [SetUp]
        public void Setup()
        {
            _serviceStub = new Mock<IUsersService>();
            _controller = new UsersController(_serviceStub.Object);
            _users = SetUpUsers();
        }

        private IEnumerable<User> SetUpUsers()
        {
            var users = new List<User>();

            for (int i = 1; i <= 3; i++)
            {
                users.Add(CreateNewUser(i));
            }

            return users.AsEnumerable();
        }

        private User CreateNewUser(long id)
        {
            return new()
            {
                Id = id,
                Email = "userTest" + id + "@test.com",
                FirstName = "User" + id,
                LastName = "Test" + id,
                Password = "password" + id
            };
        }

        [Test]
        public void Get_WhenItemIsNull_ReturnsNotFound()
        {
            //Arrange            
            _serviceStub.Setup(service => service.GetById(0));

            //Act
            var result = _controller.Get(0);

            //Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        [TestCase(1)]
        public void Get_WhenItemExists_ReturnsExpectedItem(long id)
        {
            //Arrange            
            _serviceStub.Setup(service => service.GetById(id)).Returns(CreateNewUser(id));

            //Act
            var result = _controller.Get(id);

            //Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            User userReturned = (User)(result.Result as OkObjectResult).Value;
            Assert.That(id, Is.EqualTo(userReturned.Id));
        }

        [Test]
        public void GetAll_WhenEmptyList_ReturnsNoContent()
        {
            //Arrange
            _serviceStub.Setup(service => service.GetAll());

            //Act
            var result = _controller.GetAll();

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result.Result);
        }

        [Test]
        public void GetAll_WhenListIsNotEmpty_ReturnsOk()
        {
            //Arrange
            _serviceStub.Setup(service => service.GetAll()).Returns(_users);

            //Act
            var result = _controller.GetAll();

            //Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            Assert.That(_users.Count, Is.EqualTo(((result.Result as OkObjectResult).Value as IEnumerable<User>).Count()));
        }

        //[Test]
        //[TestCase(1)]
        //public void DeleteAsync_WhenItemExists_ReturnOK(long id)
        //{
        //    //Arrange
        //    var user = CreateNewUser(id);
        //    _serviceStub.Setup(service => service.Delete(user));

        //    //Act
        //    var result = _controller.Delete(id);

        //    Assert.That(result, Is.InstanceOf<OkObjectResult>());
        //}
    }
}
