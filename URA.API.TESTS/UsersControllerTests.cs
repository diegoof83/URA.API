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
            var requestUri = "http://localhost/api/ura/users";

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
            var requestUri = "http://localhost/api/ura/users/";

            //Act
            var response = await _client.DeleteAsync(requestUri + id.ToString());

            //Assert            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAll_WhenEmptyList_ReturnsNoContent()
        {
            //Arrange
            var requestUri = "http://localhost/api/ura/users";

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
            var requestUri = "http://localhost/api/ura/users/";

            //Act
            var response = await _client.GetAsync(requestUri + id.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var userReturned = await response.Content.ReadAsAsync<User>();
            userReturned.Should().NotBeNull();
            userReturned.Id.Should().Be(id);
        }

        [Fact]
        public async Task Get_WhenItemDoesNotExists_ReturnsNotFound()
        {
            //Arrange
            var requestUri = "http://localhost/api/ura/users/";

            //Act
            var response = await _client.GetAsync(requestUri + "0");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var userReturned = await response.Content.ReadAsAsync<User>();
            userReturned.Should().BeNull();
        }
    }
}
