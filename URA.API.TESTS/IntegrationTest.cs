using System;
using System.Net.Http;
using Xunit;

namespace URA.API.TESTS
{
    public class IntegrationTest : IClassFixture<ApiWebApplicationFactory<Startup>>
    {
        protected readonly ApiWebApplicationFactory<Startup> _factory;
        protected readonly HttpClient _client;
        protected Uri _baseAddress;

        public IntegrationTest(ApiWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _baseAddress = new Uri(_client.BaseAddress.ToString() + "api/ura/users");
        }
    }
}
