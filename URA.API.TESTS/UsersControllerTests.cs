using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using URA.API.Domain.Models;
using Xunit;

namespace URA.API.TESTS
{
    public class UsersControllerTests : IntegrationTest
    {
        public UsersControllerTests(ApiWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetAll_WhenExistOneOrMoreUsers_ReturnsOkWithFullList()
        {
            //Arrange
            var requestUri = _baseAddress;

            //Act
            var response = await _client.GetAsync(requestUri);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<IEnumerable<User>>()).Should().HaveCount(3);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteAsync_WhenItemExists_ReturnOk(long id)
        {
            //Arrange
            var requestUri = _baseAddress + "/" + id;

            //Act
            var response = await _client.DeleteAsync(requestUri);

            //Assert            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(0)]
        public async Task DeleteAsync_WhenItemDoesNotExists_ReturnNotFound(long id)
        {
            //Arrange
            var requestUri = _baseAddress + "/" + id;

            //Act
            var response = await _client.DeleteAsync(requestUri);

            //Assert            
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAll_WhenEmptyList_ReturnsNoContent()
        {
            //Arrange
            var requestUri = _baseAddress;

            //Act
            var response = await _client.GetAsync(requestUri);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            (await response.Content.ReadAsAsync<IEnumerable<User>>()).Should().BeEmpty();
        }

        [Theory]
        [InlineData(2)]
        public async Task Get_WhenItemExists_ReturnsOkWithExpectedItem(long id)
        {
            //Arrange
            var requestUri = _baseAddress + "/" + id;

            //Act
            var response = await _client.GetAsync(requestUri);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var userReturned = await response.Content.ReadAsAsync<User>();
            userReturned.Should().NotBeNull();
            userReturned.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(0)]
        public async Task Get_WhenItemDoesNotExists_ReturnsNotFound(long id)
        {
            //Arrange
            var requestUri = _baseAddress + "/" + id;

            //Act
            var response = await _client.GetAsync(requestUri);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_WhenCreateNewUser_ReturnsCreatedResultWithNewUser()
        {
            //Arrange
            var requestUri = _baseAddress;
            var newUser = DbForTestsInitializer.CreateNewUser(0);

            //Act
            var response = await _client.PostAsJsonAsync(requestUri, newUser);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var userReturned = await response.Content.ReadAsAsync<User>();
            userReturned.Should().NotBeNull();
            userReturned.Id.Should().NotBe(0);
            userReturned.Email.Should().BeEquivalentTo(newUser.Email);
            userReturned.FirstName.Should().BeEquivalentTo(newUser.FirstName);
            userReturned.LastName.Should().BeEquivalentTo(newUser.LastName);
        }

        [Theory]
        [InlineData(0)]
        public async Task Put_WhenUpdateNotExistingUser_ReturnsNotFound(long id)
        {
            //Arrange
            var requestUri = _baseAddress + "/" + id;
            var userToUpdate = DbForTestsInitializer.CreateNewUser(id);

            //Act
            var response = await _client.PutAsJsonAsync(requestUri, userToUpdate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(3)]
        public async Task Put_WhenUpdateExistingUser_ReturnsOkWithUpdatedUser(long id)
        {
            //Arrange
            var requestUri = _baseAddress + "/" + id; 
            var existingUser = DbForTestsInitializer.CreateNewUser(id);// creates a user that already exists
            var userToUpdate = existingUser; //fills the user with new data
            userToUpdate.Email = "Updated Email";            
            userToUpdate.FirstName = "Updated First Name";

            //Act
            var response = await _client.PutAsJsonAsync(requestUri, userToUpdate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var userReturned = await response.Content.ReadAsAsync<User>();
            userReturned.Should().NotBeNull();
            userReturned.Id.Should().Be(id);
            userReturned.Email.Should().BeEquivalentTo(userToUpdate.Email);
            userReturned.FirstName.Should().BeEquivalentTo(userToUpdate.FirstName);
            userReturned.LastName.Should().BeEquivalentTo(existingUser.LastName);//Last name was not updated
        }
    }
}
