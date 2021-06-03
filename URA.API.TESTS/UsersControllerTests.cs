using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URA.API.Controllers;
using URA.API.Domain.Models;
using URA.API.Domain.Services;
using URA.API.Persistence.Contexts;
using URA.API.Persistence.Repositories;
using URA.API.Services;

namespace URA.API.TESTS
{
    [TestFixture]
    public class UsersControllerTests: BaseTest
    {
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new UsersController(GetService<IUsersService>());
        }

        /// <summary>
        /// Create a new list of Users with 3 new Users objects
        /// </summary>
        /// <returns>A list of Users as Enumerable</returns>
        private IEnumerable<User> SetUpUsers()
        {
            var users = new List<User>();

            for (int i = 1; i <= 3; i++)
            {
                users.Add(CreateNewUser(i));
            }

            return users.AsEnumerable();
        }

        /// <summary>
        /// Create a new User object following the parameter id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        public void Post_WhenUserRequireCreation_ReturnsANewUser()
        {
            var result = _controller.Post(CreateNewUser(0));

            Assert.That(result.Result, Is.InstanceOf<CreatedResult>());
            var user = ((result.Result as CreatedResult).Value as User);
            Assert.That(user, Is.TypeOf<User>());
            Assert.That(user.Id, Is.GreaterThan(0));
        }

        //[Test]
        //public void Get_WhenItemIsNull_ReturnsNotFound()
        //{
        //    //Arrange
        //    _mockRepository.Setup(rep => rep.GetById(It.IsAny<int>())).Returns(new Func<int, User>(id => _users.FirstOrDefault(u => u.Id == id)));

        //    //Act
        //    var result = _controller.GetById(0);

        //    //Assert
        //    Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        //}

        //[Test]
        //[TestCase(1)]
        //public void Get_WhenItemExists_ReturnsExpectedItem(long id)
        //{
        //    //Arrange            
        //    //_serviceStub.Setup(service => service.GetById(id)).Returns(CreateNewUser(id));

        //    //Act
        //    var result = _controller.GetById(id);

        //    //Assert
        //    Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        //    User userReturned = (User)(result.Result as OkObjectResult).Value;
        //    Assert.That(id, Is.EqualTo(userReturned.Id));
        //}

        //[Test]
        //public void GetAll_WhenEmptyList_ReturnsNoContent()
        //{
        //    //Arrange
        //    // _serviceStub.Setup(service => service.GetAll());

        //    //Act
        //    var result = _controller.GetAll();

        //    //Assert
        //    Assert.IsInstanceOf<NoContentResult>(result.Result);
        //}

        //[Test]
        //public void GetAll_WhenListIsNotEmpty_ReturnsOk()
        //{
        //    //Arrange
        //    //_serviceStub.Setup(service => service.GetAll()).Returns(_users);

        //    //Act
        //    var result = _controller.GetAll();

        //    //Assert
        //    Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        //    Assert.That(_users.Count, Is.EqualTo(((result.Result as OkObjectResult).Value as IEnumerable<User>).Count()));
        //}

        //[Test]
        //[TestCase(1)]
        //public void DeleteAsync_WhenItemExists_ReturnOK(long id)
        //{
        //    //Arrange
        //    var user = CreateNewUser(id);
        //    _serviceStub.Setup(service => service.Delete(user));

        //_mockRepository.Setup(rep => rep.Delete(It.IsAny<User>()))
        //        .Callback(new Action<User>(user => 
        //        {
        //        var userToRemove = _users.FirstOrDefault(u => u.Id == user.Id);

        //        if (userToRemove != null)
        //            _users.Remove(userToRemove);
        //        }
        //        ));

        //    //Act
        //    var result = _controller.Delete(id);

        //    Assert.That(result, Is.InstanceOf<OkObjectResult>());
        //}
    }
}
