using System.Net.Http;
using Xunit;

namespace URA.API.TESTS
{
    public class IntegrationTest : IClassFixture<ApiWebApplicationFactory<Startup>>
    {
        protected readonly ApiWebApplicationFactory<Startup> _factory;
        protected readonly HttpClient _client;
        protected string _base;

        public IntegrationTest(ApiWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
    }
}
